using Microsoft.EntityFrameworkCore;
using MWS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Persistens.Repositories;

public class ResourceQuantityAggregateRepository : IResourceQuantityAggregateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ResourceQuantityAggregateRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(ResourceQuantityAggregate entity) =>
        await _dbContext.ResourceQuantityAggregates.AddAsync(entity);

    public Task DeleteAsync(ResourceQuantityAggregate entity)
    {
        _dbContext.ResourceQuantityAggregates.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ResourceQuantityAggregate>> GetAll() =>
        await _dbContext.ResourceQuantityAggregates
            .AsNoTracking()
            .ToListAsync();

    public async Task<ResourceQuantityAggregate?> GetByIdAsync(Guid id) =>
        await _dbContext.ResourceQuantityAggregates
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<ResourceQuantityAggregate?> GetByResourceAndUnitAsync(Guid resourceId, Guid unitOfMeasureId) =>
        await _dbContext.ResourceQuantityAggregates
            .FirstOrDefaultAsync(a => a.ResourceId == resourceId && a.UnitOfMeasureId == unitOfMeasureId);

    public async Task<ResourceQuantityAggregate?> GetByResourceAsync(Guid resourceId) =>
        await _dbContext.ResourceQuantityAggregates
            .FirstOrDefaultAsync(a => a.ResourceId == resourceId);

    public async Task<ResourceQuantityAggregate?> GetByUnitOfMeasureAsync(Guid unitOfMeasureId) =>
        await _dbContext.ResourceQuantityAggregates
            .FirstOrDefaultAsync(a => a.UnitOfMeasureId == unitOfMeasureId);

    public Task UpdateAsync(ResourceQuantityAggregate entity)
    {
        _dbContext.ResourceQuantityAggregates.Update(entity);
        return Task.CompletedTask;
    }
}