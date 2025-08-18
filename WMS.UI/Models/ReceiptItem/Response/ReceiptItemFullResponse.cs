using WMS.UI.Models.Resource.Response;
using WMS.UI.Models.UnitOfMeasure.Response;

namespace WMS.UI.Models.ReceiptItem.Response;

public record ReceiptItemFullResponse(
    Guid Id,
    Guid ResourceId,
    ResourceResponse Resource,
    Guid UnitOfMeasureId,
    UnitOfMeasureResponse UnitOfMeasure,
    decimal Quantity);