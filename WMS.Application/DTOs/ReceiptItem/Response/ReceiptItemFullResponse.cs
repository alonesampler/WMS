namespace WMS.Application.DTOs.ReceiptItem.Response;

public record ReceiptItemFullResponse(
    Guid Id,
    Guid ResourceId,
    string ResourceTitle,
    Guid UnitOfMeasureId,
    string UnitOfMeasureTitle,
    decimal Quantity);