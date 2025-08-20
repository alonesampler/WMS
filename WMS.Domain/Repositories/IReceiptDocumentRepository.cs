using WMS.Domain.Entities;

namespace WMS.Domain.Repositories;

public interface IReceiptDocumentRepository : IRepository<ReceiptDocument>
{
    Task<IEnumerable<ReceiptDocument>> GetAllWithFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? applicationNumberFilter = null,
        List<Guid>? resourceIds = null, // Изменили на ID ресурсов
        List<Guid>? unitOfMeasureIds = null);
}
