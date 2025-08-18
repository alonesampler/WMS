using WMS.UI.Models.ReceiptItem.Response;

namespace WMS.UI.Models.ReceiptDocument;

public record ReceiptDocument(Guid Id, string ApplicationNumber, DateTime Date, List<ReceiptItemFullResponse> Items);