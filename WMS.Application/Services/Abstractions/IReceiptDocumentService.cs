using FluentResults;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Domain.Entities;

namespace WMS.Application.Services.Abstractions;
public interface IReceiptDocumentService
{
    Task<Result> CreateAsync(ReceiptDocumentParamsRequest @params);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<List<ReceiptDocument>>> GetAllWithFiltersAsync(DateTime? startDate = null, DateTime? endDate = null, string? applicationNumberFilter = null);
    Task<Result<ReceiptDocument>> GetByIdAsync(Guid id);
    Task<Result> UpdateAsync(Guid id, ReceiptDocumentParamsRequest @params);
}