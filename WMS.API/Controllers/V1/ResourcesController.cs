using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Resource.Request;
using WMS.Application.DTOs.Resource.Responce;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.API.Controllers.V1;


[ApiController]
[Route("v1/Resources")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceService _resourceService;
    
    public ResourcesController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] ResourceParamsRequest @params)
    {
        var result = await _resourceService.CreateAsync(@params);
        
        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Resource>> GetByIdAsync(Guid id)
    {
        var result = await _resourceService.GetByIdAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors.Select(e => e.Message));
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByState([FromQuery] State state = State.Working)
    {
        var result = await _resourceService.GetByStateAsync(state);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ResourceParamsRequest @params)
    {
        var result = await _resourceService.UpdateAsync(id, @params);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
    
    [HttpPost("{id:guid}/archive")]
    public async Task<IActionResult> Archive(Guid id)
    {
        var result = await _resourceService.ArchiveResourceAsync(id);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
    
    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _resourceService.RestoreResourceAsync(id);
        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _resourceService.DeleteAsync(id);
        
        if (result.IsFailed)
            return NotFound(result.Errors);

        return NoContent();
    }
}