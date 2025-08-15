using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;

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
    public async Task<ActionResult<ReceiptDocument>> GetByIdAsync(Guid id)
    {
        var result = await _receiptService.GetByIdAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));
        
        return Ok(result.Value);
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
        [FromQuery] string? resourceTitleFilter = null,
        [FromQuery] string? unitOfMeasureTitleFilter = null)
    {
        var result = await _receiptService.GetAllWithFiltersAsync(
            startDate,
            endDate,
            applicationNumberFilter,
            resourceTitleFilter,
            unitOfMeasureTitleFilter);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));
        
        return Ok(result.Value);
    }
}