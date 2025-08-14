using MWS.Domain.Exceptions;

namespace MWS.Domain.Entities;

public class ResourceQuantityAggregate
{
    private ResourceQuantityAggregate() { }

    private ResourceQuantityAggregate(Guid id, Guid resourceId, Guid unitOfMeasureId, decimal totalQuantity)
    {
        Id = id;
        ResourceId = resourceId;
        UnitOfMeasureId = unitOfMeasureId;
        TotalQuantity = totalQuantity;
    }

    public Guid Id { get; private set; }

    public Guid ResourceId { get; private set; }

    public Guid UnitOfMeasureId { get; private set; }

    public decimal TotalQuantity { get; private set; }

    public static ResourceQuantityAggregate Create(
        Guid id,
        Guid resourceId,
        Guid unitOfMeasureId,
        decimal totalQuantity)
    {
        if (id == Guid.Empty)
            throw new DomainException("Aggregate Id is empty");

        if (resourceId == Guid.Empty)
            throw new DomainException("Resource Id is empty");

        if (unitOfMeasureId == Guid.Empty)
            throw new DomainException("Unit of Measure Id is empty");

        if (totalQuantity <= 0)
            throw new DomainException("Total quantity must be greater than zero");

        return new ResourceQuantityAggregate(id, resourceId, unitOfMeasureId, totalQuantity);
    }
    public void UpdateTotalQuantity(decimal totalQuantity)
    {
        if (totalQuantity < 0)
            throw new DomainException("Total quantity can not be lower zero");

        TotalQuantity = totalQuantity;
    }
}
