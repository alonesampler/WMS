using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Persistens.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ResourceRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Resource?> GetByIdAsync(Guid id) =>
        await _dbContext.Resources
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(Resource entity) =>
        await _dbContext.Resources.AddAsync(entity);


    public Task UpdateAsync(Resource entity)
    {
        _dbContext.Resources.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Resource entity)
    {
        _dbContext.Resources.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Resource>> GetByStateWithFiltersAsync(State state, string? search = null)
    {
        var query = _dbContext.Resources
            .Where(r => r.State == state)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string lowerSearch = search.ToLower();
            query = query.Where(r => r.Title.ToLower().Contains(lowerSearch));
        }

        return await query.ToListAsync();
    }

    public async Task<Resource?> GetByTitleAsync(string title) =>
        await _dbContext.Resources
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Title.ToLower() == title.ToLower());
}