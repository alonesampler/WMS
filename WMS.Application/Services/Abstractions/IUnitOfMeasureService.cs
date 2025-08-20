using FluentResults;
using WMS.Application.DTOs.UnitOfMeasure.Request;
using WMS.Application.DTOs.UnitOfMeasure.Response;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Application.Services.Abstractions;

public interface IUnitOfMeasureService
{
    Task<Result> CreateAsync(UnitOfMeasureParamsRequest @params);
    Task<Result> ArchiveResourceAsync(Guid id);
    Task<Result> RestoreResourceAsync(Guid id);
    Task<Result> UpdateAsync(Guid id, UnitOfMeasureParamsRequest @params);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<UnitOfMeasure>> GetByIdAsync(Guid id);
    Task<Result<List<UnitOfMeasureResponse>>> GetByStateAsync(State state, string? search = null);
}