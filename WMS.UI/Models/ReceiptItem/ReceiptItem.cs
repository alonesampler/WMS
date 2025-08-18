namespace WMS.UI.Models.ReceiptItem;

public record ReceiptItem(Guid Id, Guid ResourceId, Guid UnitOfMeasureId, decimal Quantity);