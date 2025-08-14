using WMS.Domain.Entities;

namespace WMS.Domain.Repositories;

public interface IReceiptItemRepository : IRepository<ReceiptItem>
{
    Task<IEnumerable<ReceiptItem>> GetAllByReceiptDocumentIdAsync(Guid receiptDocumentId);
}
