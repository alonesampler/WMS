namespace WMS.UI.Models.ReceiptItem.Request;

public record ReceiptItemParamsRequest(Guid ResourceId, Guid UnitOfMeasureId, decimal Quantity);