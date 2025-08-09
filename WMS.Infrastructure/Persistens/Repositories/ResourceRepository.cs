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

    public async Task<List<Resource>> GetByStateAsync(State state) =>
        await _dbContext.Resources
            .AsNoTracking()
            .Where(r => r.State == state)
            .ToListAsync();
}