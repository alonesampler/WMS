using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Resource.Request;
using WMS.Application.DTOs.Resource.Responce;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.API.Controllers.V1;


[ApiController]
[Route("v1/Resource")]
public class ResourcesController : ControllerBase
{
    private readonly IResourceService _resourceService;
    
    public ResourcesController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpPost("new")]
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
    
    [HttpGet("archived")]
    public async Task<ActionResult<List<ResourceResponse>>> GetArchivedAsync(State state = State.Archived)
    {
        var result = await _resourceService.GetArchivedAsync();
        
        return Ok(result.Value);
    }
    
    
}