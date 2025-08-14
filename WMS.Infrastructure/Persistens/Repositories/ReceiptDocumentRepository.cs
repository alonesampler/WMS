using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Persistens.Repositories;

public class ReceiptDocumentRepository : IReceiptDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ReceiptDocumentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReceiptDocument?> GetByIdAsync(Guid id) =>
        await _dbContext.ReceiptDocuments
            .Include(d => d.Items)
                .ThenInclude(i => i.Resource)
            .Include(d => d.Items)
                .ThenInclude(i => i.UnitOfMeasure)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(ReceiptDocument entity) =>
        await _dbContext.ReceiptDocuments.AddAsync(entity);

    public Task UpdateAsync(ReceiptDocument entity)
    {
        _dbContext.ReceiptDocuments.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ReceiptDocument entity)
    {
        _dbContext.ReceiptDocuments.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ReceiptDocument>> GetAllWithFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? applicationNumberFilter = null,
        string? resourceTitleFilter = null,
        string? unitOfMeasureTitleFilter = null)
    {
        var query = _dbContext.ReceiptDocuments
            .Include(d => d.Items)
                .ThenInclude(i => i.Resource)
            .Include(d => d.Items)
                .ThenInclude(i => i.UnitOfMeasure)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(d => d.Date >= startDate);

        if (endDate.HasValue)
            query = query.Where(d => d.Date <= endDate);

        if (!string.IsNullOrWhiteSpace(applicationNumberFilter))
            query = query.Where(d => d.ApplicationNumber.Contains(applicationNumberFilter));

        if (!string.IsNullOrWhiteSpace(resourceTitleFilter))
            query = query.Where(d => d.Items.Any(i => i.Resource.Title.Contains(resourceTitleFilter)));

        if (!string.IsNullOrWhiteSpace(unitOfMeasureTitleFilter))
            query = query.Where(d => d.Items.Any(i => i.UnitOfMeasure.Title.Contains(unitOfMeasureTitleFilter)));

        return await query.AsNoTracking().ToListAsync();
    }
}