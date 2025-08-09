using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Persistens.Repositories;

public class UnitOfMeasureRepository : IUnitOfMeasureRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfMeasureRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<UnitOfMeasure?> GetByIdAsync(Guid id) =>
        await _dbContext.UnitOfMeasures
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(UnitOfMeasure entity) =>
        await _dbContext.UnitOfMeasures.AddAsync(entity);

    public Task UpdateAsync(UnitOfMeasure entity)
    {
        _dbContext.UnitOfMeasures.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(UnitOfMeasure entity)
    {
        _dbContext.UnitOfMeasures.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<List<UnitOfMeasure>> GetByStateAsync(State state) =>
        _dbContext.UnitOfMeasures
            .AsNoTracking()
            .Where(r => r.State == state)
            .ToListAsync();
}