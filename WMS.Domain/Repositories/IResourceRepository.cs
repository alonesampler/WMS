using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Domain.Repositories;

public interface IResourceRepository : IRepository<Resource>
{
    public Task<List<Resource>> GetByStateAsync(State state);
}
