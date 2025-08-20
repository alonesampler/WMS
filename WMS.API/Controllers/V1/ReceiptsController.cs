using Microsoft.AspNetCore.Mvc;
using WMS.API.Controllers.Extensions;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.Application.Services.Abstractions;

namespace WMS.API.Controllers.V1;

[ApiController]
[Route("api/v1/receipts")]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptsService _receiptService;

    public ReceiptsController(IReceiptsService receiptService)
    {
        _receiptService = receiptService;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] ReceiptDocumentParamsRequest @params)
    {
        var result = await _receiptService.CreateAsync(@params);
        
        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReceiptDocumentFullResponse>> GetByIdAsync(Guid id)
    {
        var result = await _receiptService.GetByIdAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));
        
        var document = result.Value;
        var response = document.ToResponse();
        
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _receiptService.DeleteAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));

        return NoContent();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] ReceiptDocumentParamsRequest @params)
    {
        var result = await _receiptService.UpdateAsync(id, @params);
        
        if (result.IsFailed)
        {
            return result.Errors.Any(e => e.Message == "Receipt document not found") 
                ? NotFound(result.Errors.Select(e => e.Message)) 
                : BadRequest(result.Errors.Select(e => e.Message));
        }

        return NoContent();
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ReceiptDocumentInfoResponse>>> GetAllWithFilters(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? applicationNumberFilter = null,
        [FromQuery] List<Guid>? resourceIds = null, // Меняем на ID
        [FromQuery] List<Guid>? unitOfMeasureIds = null) // Меняем на ID
    {
        var result = await _receiptService.GetAllWithFiltersAsync(
            startDate,
            endDate,
            applicationNumberFilter,
            resourceIds, // Передаем ID
            unitOfMeasureIds); // Передаем ID

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));
    
        return Ok(result.Value);
    }
}