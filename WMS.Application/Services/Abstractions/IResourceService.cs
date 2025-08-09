using FluentResults;
using WMS.Application.DTOs.Resource.Request;
using WMS.Application.DTOs.Resource.Responce;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Application.Services.Abstractions;

public interface IResourceService
{
    Task<Result> CreateAsync(ResourceParamsRequest @params);
    Task<Result> ArchiveResourceAsync(Guid id);
    Task<Result> RestoreResourceAsync(Guid id);
    Task<Result> UpdateAsync(Guid id, ResourceParamsRequest @params);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<Resource>> GetByIdAsync(Guid id);
    Task<Result<List<ResourceResponse>>> GetByStateAsync(State state);
}