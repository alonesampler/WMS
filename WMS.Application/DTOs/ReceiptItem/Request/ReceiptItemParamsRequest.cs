namespace WMS.Application.DTOs.ReceiptItem.Request;

public record ReceiptItemParamsRequest(Guid ResourceId, Guid UnitOfMeasureId, decimal Quantity);