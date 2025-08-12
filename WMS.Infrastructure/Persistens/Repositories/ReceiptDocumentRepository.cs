using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Domain.UnitOfWork;

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

    public async Task<IEnumerable<ReceiptDocument>> GetAllWithFiltersAsync(DateTime? startDate = null, DateTime? endDate = null,
        string? applicationNumberFilter = null)
    {
        var query = _dbContext.ReceiptDocuments.AsQueryable();
        
        if(startDate.HasValue)
            query = query.Where(r => r.Date >= startDate.Value);

        if (endDate.HasValue)
        {
            var endDateInclusive = endDate.Value.Date.AddDays(1);
            query = query.Where(r => r.Date < endDate.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(applicationNumberFilter))
        {
            query = query.Where(x => x.ApplicationNumber.Contains(applicationNumberFilter));
        }
        
        query = query.OrderByDescending(x => x.Date);
        
        return await query.ToListAsync();
    }
}