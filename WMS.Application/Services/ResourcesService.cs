using FluentResults;
using WMS.Application.DTOs.Resource.Request;
using WMS.Application.DTOs.Resource.Response;
using WMS.Application.Extensions;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.UnitOfWork;

namespace WMS.Application.Services;

public class ResourcesService : IResourceService
{
    private readonly IUnitOfWork _unitOfWork;

    public ResourcesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateAsync(ResourceParamsRequest @params)
    {
        var existingUnit = await _unitOfWork.ResourceRepository.GetByTitleAsync(@params.Title);
        
        if (existingUnit != null)
        {
            return Result.Fail("Ресурс с таким названием уже существует");
        }
        
        var state = (State)Resource.DEFAULT_STATE_NUMBER;

        var resource = Resource.Create(
            Guid.NewGuid(),
            @params.Title,
            state);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ResourceRepository.CreateAsync(resource);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> ArchiveResourceAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var resource = getResult.Value;

        if (resource.State == State.Archived)
            return Result.Fail("Resource already archived");

        resource.Archive();

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ResourceRepository.UpdateAsync(resource);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> RestoreResourceAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var resource = getResult.Value;
        
        resource.Restore();

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ResourceRepository.UpdateAsync(resource);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(Guid id, ResourceParamsRequest @params)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var resource = getResult.Value;
        
        if (!resource.Title.Equals(@params.Title, StringComparison.OrdinalIgnoreCase))
        {
            var existingUnit = await _unitOfWork.UnitOfMeasureRepository.GetByTitleAsync(@params.Title);
            
            if (existingUnit != null && existingUnit.Id != id)
            {
                return Result.Fail("Ресурс с таким названием уже существует");
            }
        }

        resource.Update(@params.Title);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ResourceRepository.UpdateAsync(resource);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var resource = getResult.Value;

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ResourceRepository.DeleteAsync(resource);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result<Resource>> GetByIdAsync(Guid id)
    {
        var resource = await _unitOfWork.ResourceRepository.GetByIdAsync(id);
        
        if (resource == null)
            return Result.Fail("Resource not found");
        
        return Result.Ok(resource);
    }
    
    public async Task<Result<List<ResourceResponse>>> GetByStateAsync(State state, string? search = null)
    {
        var resources = await _unitOfWork.ResourceRepository
            .GetByStateWithFiltersAsync(state, search);

        var response = resources.Select(r => r.ToResponse()).ToList();

        return Result.Ok(response);
    }
}
