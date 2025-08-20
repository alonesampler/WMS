using MWS.Domain.Entities;

namespace WMS.Domain.Repositories;

public interface IResourceQuantityAggregateRepository : IRepository<ResourceQuantityAggregate>
{
    public Task<IEnumerable<ResourceQuantityAggregate>> GetAll();

    public Task<ResourceQuantityAggregate?> GetByUnitOfMeasureAsync(Guid unitOfMeasureId);

    public Task<ResourceQuantityAggregate?> GetByResourceAsync(Guid resourceId);

    public Task<ResourceQuantityAggregate?> GetByResourceAndUnitAsync(Guid resourceId, Guid unitOfMeasureId);
}
