using FluentResults;
using WMS.Application.DTOs.UnitOfMeasure.Request;
using WMS.Application.DTOs.UnitOfMeasure.Response;
using WMS.Application.Extensions;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.UnitOfWork;

namespace WMS.Application.Services;

public class UnitOfMeasuresService : IUnitOfMeasureService
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfMeasuresService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateAsync(UnitOfMeasureParamsRequest @params)
    {
        var state = (State)UnitOfMeasure.DEFAULT_STATE_NUMBER;

        var unitOfMeasure = UnitOfMeasure.Create(
            Guid.NewGuid(),
            @params.Title,
            state);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UnitOfMeasureRepository.CreateAsync(unitOfMeasure);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> ArchiveResourceAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var unitOfMeasure = getResult.Value;

        if (unitOfMeasure.State == State.Archived)
            return Result.Fail("Unit of measure already archived");

        unitOfMeasure.Archive();

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UnitOfMeasureRepository.UpdateAsync(unitOfMeasure);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> RestoreResourceAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var unitOfMeasure = getResult.Value;
        
        unitOfMeasure.Restore();

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UnitOfMeasureRepository.UpdateAsync(unitOfMeasure);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(Guid id, UnitOfMeasureParamsRequest @params)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var unitOfMeasure = getResult.Value;

        unitOfMeasure.Update(@params.Title);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UnitOfMeasureRepository.UpdateAsync(unitOfMeasure);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);
        
        if (getResult.IsFailed)
            return getResult.ToResult();

        var unitOfMeasure = getResult.Value;

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UnitOfMeasureRepository.DeleteAsync(unitOfMeasure);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result<UnitOfMeasure>> GetByIdAsync(Guid id)
    {
        var unitOfMeasure = await _unitOfWork.UnitOfMeasureRepository.GetByIdAsync(id);
        
        if (unitOfMeasure == null)
            return Result.Fail("Unit of measure not found");
        
        return Result.Ok(unitOfMeasure);
    }
    
    public async Task<Result<List<UnitOfMeasureResponse>>> GetByStateAsync(State state)
    {
        var unitOfMeasures = await _unitOfWork.UnitOfMeasureRepository.GetByStateWithFiltersAsync(state);

        var response = unitOfMeasures.Select(r => r.ToResponse()).ToList();

        return Result.Ok(response);
    }
}