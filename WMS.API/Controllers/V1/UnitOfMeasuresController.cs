using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.UnitOfMeasure.Request;
using WMS.Application.DTOs.UnitOfMeasure.Response;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.API.Controllers.V1;

[ApiController]
[Route("api/v1/unit-of-measures")]
public class UnitOfMeasuresController : ControllerBase
{
    private readonly IUnitOfMeasureService _unitOfMeasureService;

    public UnitOfMeasuresController(IUnitOfMeasureService unitOfMeasureService)
    {
        _unitOfMeasureService = unitOfMeasureService;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] UnitOfMeasureParamsRequest @params)
    {
        var result = await _unitOfMeasureService.CreateAsync(@params);
        
        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UnitOfMeasure>> GetByIdAsync(Guid id)
    {
        var result = await _unitOfMeasureService.GetByIdAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UnitOfMeasureResponse>>> GetByState([FromQuery] State state = State.Working)
    {
        var result = await _unitOfMeasureService.GetByStateAsync(state);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UnitOfMeasureParamsRequest @params)
    {
        var result = await _unitOfMeasureService.UpdateAsync(id, @params);
        if (result.IsFailed)
        {
            return result.Errors.Any(e => e.Message == "Unit of measure not found") 
                ? NotFound(result.Errors) 
                : BadRequest(result.Errors);
        }

        return NoContent();
    }
    
    [HttpPost("{id:guid}/archive")]
    public async Task<ActionResult> Archive(Guid id)
    {
        var result = await _unitOfMeasureService.ArchiveResourceAsync(id);
        if (result.IsFailed)
        {
            return result.Errors.Any(e => e.Message == "Resource not found" || e.Message == "Unit of measure already archived") 
                ? NotFound(result.Errors) 
                : BadRequest(result.Errors);
        }

        return NoContent();
    }
    
    [HttpPost("{id:guid}/restore")]
    public async Task<ActionResult> Restore(Guid id)
    {
        var result = await _unitOfMeasureService.RestoreResourceAsync(id);
        if (result.IsFailed)
        {
            return result.Errors.Any(e => e.Message == "Unit of measure not found") 
                ? NotFound(result.Errors) 
                : BadRequest(result.Errors);
        }

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _unitOfMeasureService.DeleteAsync(id);
        
        if (result.IsFailed)
        {
            return result.Errors.Any(e => e.Message == "Unit of measure not found") 
                ? NotFound(result.Errors) 
                : BadRequest(result.Errors);
        }

        return NoContent();
    }
}