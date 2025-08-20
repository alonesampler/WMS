using FluentResults;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.Domain.Entities;

namespace WMS.Application.Services.Abstractions;
public interface IReceiptsService
{
    Task<Result> CreateAsync(ReceiptDocumentParamsRequest @params);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<List<ReceiptDocumentInfoResponse>>> GetAllWithFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? applicationNumberFilter = null,
        List<Guid>? resourceIds = null,
        List<Guid>? unitOfMeasureIds = null);
    Task<Result<ReceiptDocument>> GetByIdAsync(Guid id);
    Task<Result> UpdateAsync(Guid id, ReceiptDocumentParamsRequest @params);
}