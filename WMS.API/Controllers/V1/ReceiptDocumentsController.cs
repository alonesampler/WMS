using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;

namespace WMS.API.Controllers.V1;

[ApiController]
[Route("v1/ReceiptDocuments")]
public class ReceiptDocumentsController : ControllerBase
{
    private readonly IReceiptDocumentService _receiptDocumentService;

    public ReceiptDocumentsController(IReceiptDocumentService receiptDocumentService)
    {
        _receiptDocumentService = receiptDocumentService;
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] ReceiptDocumentParamsRequest @params)
    {
        var result = await _receiptDocumentService.CreateAsync(@params);
        
        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReceiptDocument>> GetByIdAsync(Guid id)
    {
        var result = await _receiptDocumentService.GetByIdAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));
        
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _receiptDocumentService.DeleteAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));

        return NoContent();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] ReceiptDocumentParamsRequest @params)
    {
        var result = await _receiptDocumentService.UpdateAsync(id, @params);
        
        if (result.IsFailed)
        {
            return result.Errors.Any(e => e.Message == "Receipt document not found") 
                ? NotFound(result.Errors.Select(e => e.Message)) 
                : BadRequest(result.Errors.Select(e => e.Message));
        }

        return NoContent();
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ReceiptDocument>>> GetAllWithFilters(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? applicationNumber = null)
    {
        var result = await _receiptDocumentService.GetAllWithFiltersAsync(
            startDate,
            endDate,
            applicationNumber);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors.Select(e => e.Message));
        }
        
        return Ok(result.Value);
    }
}