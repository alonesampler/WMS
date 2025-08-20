using WMS.Application.DTOs.ReceiptItem.Response;

namespace WMS.Application.DTOs.ReceiptDocument.Request;

public record ReceiptDocumentFullResponse(
    Guid Id,
    string ApplicationNumber,
    DateTime Date,
    List<ReceiptItemFullResponse> Items);