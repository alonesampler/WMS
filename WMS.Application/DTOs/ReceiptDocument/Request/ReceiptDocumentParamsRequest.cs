using WMS.Application.DTOs.ReceiptItem.Request;

namespace WMS.Application.DTOs.ReceiptDocument.Request;

public record ReceiptDocumentParamsRequest(
    string ApplicationNumber,
    DateTime Date,
    List<ReceiptItemParamsRequest> Items);
