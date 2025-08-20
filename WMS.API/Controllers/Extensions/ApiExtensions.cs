using WMS.Application.DTOs.ReceiptDocument.Request;
using WMS.Application.DTOs.ReceiptItem.Response;
using WMS.Domain.Entities;

namespace WMS.API.Controllers.Extensions;

public static class ApiExtensions
{
    public static ReceiptItemFullResponse ToResponse(this ReceiptItem receiptItem) =>
        new ReceiptItemFullResponse(
            receiptItem.Id,
            receiptItem.ResourceId,
            receiptItem.Resource.Title,
            receiptItem.UnitOfMeasureId,
            receiptItem.UnitOfMeasure.Title,
            receiptItem.Quantity);

    public static ReceiptDocumentFullResponse ToResponse(this ReceiptDocument receiptDocument) =>
        new ReceiptDocumentFullResponse(
            receiptDocument.Id,
            receiptDocument.ApplicationNumber,
            receiptDocument.Date,
            receiptDocument.Items.Select(i => i.ToResponse()).ToList());
}