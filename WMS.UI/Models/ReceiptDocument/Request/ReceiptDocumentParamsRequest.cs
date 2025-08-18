using WMS.UI.Models.ReceiptItem.Request;

namespace WMS.UI.Models.ReceiptDocument.Request;

public record ReceiptDocumentParamsRequest(
    string ApplicationNumber,
    DateTime Date,
    List<ReceiptItemParamsRequest> Items);
