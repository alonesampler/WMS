using WMS.UI.Models.ReceiptItem.Response;

namespace WMS.Application.DTOs.ReceiptDocument.Response;

public record ReceiptDocumentInfoResponse(
    Guid Id,
    string ApplicationNumber,
    DateTime Date,
    List<ReceiptItemResponse> Items);