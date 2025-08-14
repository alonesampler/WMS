using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Persistens.Repositories;

public class ReceiptItemRepository : IReceiptItemRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ReceiptItemRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(ReceiptItem entity) =>
        await _dbContext.ReceiptItems.AddAsync(entity);

    public Task DeleteAsync(ReceiptItem entity)
    {
        _dbContext.ReceiptItems.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ReceiptItem>> GetAllByReceiptDocumentIdAsync(Guid receiptDocumentId) =>
        await _dbContext.ReceiptItems
            .Include(i => i.Resource)
            .Include(i => i.UnitOfMeasure)
            .Where(i => i.ReceiptDocumentId == receiptDocumentId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<ReceiptItem?> GetByIdAsync(Guid id) =>
        await _dbContext.ReceiptItems
            .Include(i => i.Resource)
            .Include(i => i.UnitOfMeasure)
            .FirstOrDefaultAsync(i => i.Id == id);

    public Task UpdateAsync(ReceiptItem entity)
    {
        _dbContext.ReceiptItems.Update(entity);
        return Task.CompletedTask;
    }
}