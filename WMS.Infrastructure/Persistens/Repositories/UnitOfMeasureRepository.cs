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

    public async Task<IEnumerable<UnitOfMeasure>> GetByStateWithFiltersAsync(State state, string? search = null)
    {
        var query = _dbContext.UnitOfMeasures
            .Where(r => r.State == state)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string lowerSearch = search.ToLower();
            query = query.Where(r => r.Title.ToLower().Contains(lowerSearch));
        }

        return await query.ToListAsync();
    }

    public async Task<UnitOfMeasure?> GetByTitleAsync(string title) =>
        await _dbContext.UnitOfMeasures
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Title.ToLower() == title.ToLower());
    
    
}