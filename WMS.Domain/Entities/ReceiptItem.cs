using MWS.Domain.Exceptions;

namespace WMS.Domain.Entities;

public class ReceiptItem
{
    private ReceiptItem() { }

    private ReceiptItem(Guid id, Guid receiptDocumentId, Guid resourceId, Guid unitOfMeasureId, decimal quantity)
    {
        Id = id;
        ReceiptDocumentId = receiptDocumentId;
        ResourceId = resourceId;
        UnitOfMeasureId = unitOfMeasureId;
        Quantity = quantity;
    }

    public Guid Id {get; private set; }

    public Guid ReceiptDocumentId { get; private set; }
    public ReceiptDocument ReceiptDocument { get; private set; }

    public Guid ResourceId { get; private set; }
    public Resource Resource { get; private set; }

    public Guid UnitOfMeasureId {get; private set; }
    public UnitOfMeasure UnitOfMeasure { get; private set; }

    public decimal Quantity { get; private set; }

    public static ReceiptItem Create(
        Guid id,
        Guid receiptDocumentId,
        Guid resourceId,
        Guid unitOfMeasureId,
        decimal quantity)
    {
        if (id == Guid.Empty)
            throw new DomainException("Id is empty");

        if (receiptDocumentId == Guid.Empty)
            throw new DomainException("Receipt document Id is empty");

        bool hasResource = resourceId != Guid.Empty;
        bool hasUnit = unitOfMeasureId != Guid.Empty;

        if (hasResource ^ hasUnit)
            throw new DomainException("Resource and UnitOfMeasure must be specified together");

        if (hasResource && hasUnit)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");
        }

        return new ReceiptItem(id, receiptDocumentId, resourceId, unitOfMeasureId, quantity);
    }

    public void Update(Guid resourceId, Guid unitOfMeasureId, decimal quantity)
    {
        if (resourceId != ResourceId)
            ResourceId = resourceId;

        if (unitOfMeasureId != UnitOfMeasureId)
            UnitOfMeasureId = unitOfMeasureId;

        if (quantity > 0 && quantity != Quantity)
            Quantity = quantity;
    }
}