using WMS.Domain.Entities;

namespace WMS.Domain.Repositories;

public interface IReceiptDocumentRepository : IRepository<ReceiptDocument>
{
    public Task<IEnumerable<ReceiptDocument>> GetAllWithFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? applicationNumberFilter = null,
        List<Guid>? resourceIds = null,
        List<Guid>? unitOfMeasureIds = null);

    public Task<ReceiptDocument?> GetByApplicationNumber(string applicationNumber);
}
