using FluentResults;
using MWS.Domain.Entities;
using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.Application.Extensions;
using WMS.Application.Services.Abstractions;
using WMS.Domain.Entities;
using WMS.Domain.UnitOfWork;

namespace WMS.Application.Services;

public class ReceiptsService : IReceiptsService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceiptsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateAsync(ReceiptDocumentParamsRequest @params)
    {
        var existingUnit = await _unitOfWork.ReceiptDocumentRepository.GetByApplicationNumber(@params.ApplicationNumber);

        if (existingUnit != null)
        {
            return Result.Fail("Документ с таким номером заявки уже существует");
        }

        var receiptDocument = ReceiptDocument.Create(
            Guid.NewGuid(),
            @params.ApplicationNumber,
            @params.Date);

        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ReceiptDocumentRepository.CreateAsync(receiptDocument);

        foreach (var itemParam in @params.Items)
        {
            var aggregate = await _unitOfWork.ResourceQuantityAggregateRepository
                .GetByResourceAndUnitAsync(itemParam.ResourceId, itemParam.UnitOfMeasureId);

            if (aggregate == null)
            {
                aggregate = ResourceQuantityAggregate.Create(
                    Guid.NewGuid(),
                    itemParam.ResourceId,
                    itemParam.UnitOfMeasureId,
                    itemParam.Quantity);

                await _unitOfWork.ResourceQuantityAggregateRepository.CreateAsync(aggregate);
            }
            else
            {
                aggregate.UpdateTotalQuantity(aggregate.TotalQuantity + itemParam.Quantity);
            }

            var receiptItem = ReceiptItem.Create(
                Guid.NewGuid(),
                receiptDocument.Id,
                itemParam.ResourceId,
                itemParam.UnitOfMeasureId,
                itemParam.Quantity);

            await _unitOfWork.ReceiptItemRepository.CreateAsync(receiptItem);

            receiptDocument.Items.Add(receiptItem);
        }

        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }
    
    public async Task<Result<ReceiptDocument>> GetByIdAsync(Guid receiptDocumentId)
    {
        var receiptDocument = await _unitOfWork.ReceiptDocumentRepository.GetByIdAsync(receiptDocumentId);

        if (receiptDocument == null)
            return Result.Fail("Receipt document not found");

        return Result.Ok(receiptDocument);
    }

    public async Task<Result> UpdateAsync(Guid receiptDocumentId, ReceiptDocumentParamsRequest @params)
    {
        var getResult = await GetByIdAsync(receiptDocumentId);

        if (getResult.IsFailed)
            return getResult.ToResult();

        var receiptDocument = getResult.Value;

        if (!receiptDocument.ApplicationNumber.Equals(@params.ApplicationNumber, StringComparison.OrdinalIgnoreCase))
        {
            var existingUnit = await _unitOfWork.ReceiptDocumentRepository.GetByApplicationNumber(@params.ApplicationNumber);

            if (existingUnit != null && existingUnit.Id != receiptDocumentId)
                return Result.Fail("Единица измерения с таким названием уже существует");
        }

        await _unitOfWork.BeginTransactionAsync();

        receiptDocument.Update(@params.ApplicationNumber, @params.Date);

        foreach (var itemParam in @params.Items)
        {
            var existingItem = receiptDocument.Items.FirstOrDefault(i =>
                i.ResourceId == itemParam.ResourceId && i.UnitOfMeasureId == itemParam.UnitOfMeasureId);

            if (existingItem == null)
            {
                var newItem = ReceiptItem.Create(
                    Guid.NewGuid(),
                    receiptDocument.Id,
                    itemParam.ResourceId,
                    itemParam.UnitOfMeasureId,
                    itemParam.Quantity);

                receiptDocument.Items.Add(newItem);
                await _unitOfWork.ReceiptItemRepository.CreateAsync(newItem);

                var aggregate = await _unitOfWork.ResourceQuantityAggregateRepository
                    .GetByResourceAndUnitAsync(itemParam.ResourceId, itemParam.UnitOfMeasureId);

                if (aggregate == null)
                {
                    aggregate = ResourceQuantityAggregate.Create(
                        Guid.NewGuid(),
                        itemParam.ResourceId,
                        itemParam.UnitOfMeasureId,
                        itemParam.Quantity);

                    await _unitOfWork.ResourceQuantityAggregateRepository.CreateAsync(aggregate);
                }
                else
                    aggregate.UpdateTotalQuantity(aggregate.TotalQuantity + itemParam.Quantity);
            }
            else
            {
                var delta = itemParam.Quantity - existingItem.Quantity;
                existingItem.Update(existingItem.ResourceId, existingItem.UnitOfMeasureId, itemParam.Quantity);

                var aggregate = await _unitOfWork.ResourceQuantityAggregateRepository
                    .GetByResourceAndUnitAsync(itemParam.ResourceId, itemParam.UnitOfMeasureId);

                aggregate.UpdateTotalQuantity(aggregate.TotalQuantity + delta);
            }
        }

        var itemsToRemove = receiptDocument.Items
            .Where(i => !@params.Items.Any(p => p.ResourceId == i.ResourceId && p.UnitOfMeasureId == i.UnitOfMeasureId))
            .ToList();

        foreach (var itemToRemove in itemsToRemove)
        {
            var aggregate = await _unitOfWork.ResourceQuantityAggregateRepository
                .GetByResourceAndUnitAsync(itemToRemove.ResourceId, itemToRemove.UnitOfMeasureId);

            aggregate.UpdateTotalQuantity(aggregate.TotalQuantity - itemToRemove.Quantity);

            receiptDocument.Items.Remove(itemToRemove);

            await _unitOfWork.ReceiptItemRepository.DeleteAsync(itemToRemove);
        }

        await _unitOfWork.ReceiptDocumentRepository.UpdateAsync(receiptDocument);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid receiptDocumentId)
    {
        var getResult = await GetByIdAsync(receiptDocumentId);

        if (getResult.IsFailed)
            return getResult.ToResult();

        var receiptDocument = getResult.Value;

        await _unitOfWork.BeginTransactionAsync();

        foreach (var item in receiptDocument.Items)
        {
            var aggregate = await _unitOfWork.ResourceQuantityAggregateRepository
                .GetByResourceAndUnitAsync(item.ResourceId, item.UnitOfMeasureId);

            if (aggregate != null)
            {
                if (aggregate.TotalQuantity <= item.Quantity)
                    await _unitOfWork.ResourceQuantityAggregateRepository.DeleteAsync(aggregate);
                else
                    aggregate.UpdateTotalQuantity(aggregate.TotalQuantity - item.Quantity);
            }

            await _unitOfWork.ReceiptItemRepository.DeleteAsync(item);
        }

        await _unitOfWork.ReceiptDocumentRepository.DeleteAsync(receiptDocument);
        await _unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result<List<ReceiptDocumentInfoResponse>>> GetAllWithFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? applicationNumberFilter = null,
        List<Guid>? resourceIds = null,
        List<Guid>? unitOfMeasureIds = null)
    {
        try
        {
            var documents = await _unitOfWork.ReceiptDocumentRepository
                .GetAllWithFiltersAsync(startDate, endDate, applicationNumberFilter, resourceIds, unitOfMeasureIds);

            var response = documents.Select(d => d.ToResponse()).ToList();
            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка при получении документов: {ex.Message}");
        }
    }
}
