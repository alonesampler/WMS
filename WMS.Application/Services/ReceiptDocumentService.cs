using FluentResults;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.UnitOfWork;

namespace WMS.Application.Services;

public class ReceiptDocumentService : IReceiptDocumentService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceiptDocumentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> CreateAsync(ReceiptDocumentParamsRequest @params)
    {
        var receiptDocument = ReceiptDocument.Create(
            Guid.NewGuid(),
            @params.ApplicationNumber,
            @params.Date);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ReceiptDocumentRepository.CreateAsync(receiptDocument);
        await _unitOfWork.CommitAsync();

        return Result.Ok(receiptDocument.Id);
    }

    public async Task<Result<ReceiptDocument>> GetByIdAsync(Guid id)
    {
        var receiptDocument = await _unitOfWork.ReceiptDocumentRepository.GetByIdAsync(id);

        if (receiptDocument == null)
            return Result.Fail("Receipt document not found");

        return Result.Ok(receiptDocument);
    }

    public async Task<Result> UpdateAsync(Guid id, ReceiptDocumentParamsRequest @params)
    {
        var getResult = await GetByIdAsync(id);

        if (getResult.IsFailed)
            return getResult.ToResult();

        var receiptDocument = getResult.Value;

        receiptDocument.Update(@params.ApplicationNumber, @params.Date);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ReceiptDocumentRepository.UpdateAsync(receiptDocument);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var getResult = await GetByIdAsync(id);

        if (getResult.IsFailed)
            return getResult.ToResult();

        var receiptDocument = getResult.Value;

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ReceiptDocumentRepository.DeleteAsync(receiptDocument);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result<List<ReceiptDocument>>> GetAllWithFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? applicationNumberFilter = null)
    {
        var receiptDocuments = await _unitOfWork.ReceiptDocumentRepository.GetAllWithFiltersAsync(
            startDate, endDate, applicationNumberFilter);

        var receiptDocumentsList = receiptDocuments.ToList();

        return Result.Ok(receiptDocumentsList);
    }
}
