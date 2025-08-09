using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Domain.Repositories;

public interface IUnitOfMeasureRepository : IRepository<UnitOfMeasure>
{
    public Task<List<UnitOfMeasure>> GetByStateAsync(State state);
}