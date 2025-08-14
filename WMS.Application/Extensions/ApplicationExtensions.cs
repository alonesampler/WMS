using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.Application.DTOs.ReceiptItem.Response;
using WMS.Application.DTOs.Resource.Response;
using WMS.Application.DTOs.UnitOfMeasure.Response;
using WMS.Domain.Entities;

namespace WMS.Application.Extensions;

internal static class ApplicationExtensions
{
    public static ResourceResponse ToResponse(this Resource resource) =>
        new ResourceResponse(resource.Id, resource.Title);

    public static UnitOfMeasureResponse ToResponse(this UnitOfMeasure unitOfMeasure) =>
        new UnitOfMeasureResponse(unitOfMeasure.Id, unitOfMeasure.Title);

    public static ReceiptItemResponse ToResponse(this ReceiptItem receiptItem) =>
        new ReceiptItemResponse(
            receiptItem.Resource.Title,
            receiptItem.UnitOfMeasure.Title,
            receiptItem.Quantity);

    public static ReceiptDocumentInfoResponse ToResponse(this ReceiptDocument receiptDocument) =>
        new ReceiptDocumentInfoResponse(
            receiptDocument.Id,
            receiptDocument.ApplicationNumber,
            receiptDocument.Date,
            receiptDocument.Items.Select(i => i.ToResponse()).ToList());
}