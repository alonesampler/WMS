using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Domain.Repositories;

public interface IUnitOfMeasureRepository : IRepository<UnitOfMeasure>
{
    public Task<IEnumerable<UnitOfMeasure>> GetByStateWithFiltersAsync(State state, string? search = null);
}