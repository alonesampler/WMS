using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Domain.Repositories;

public interface IResourceRepository : IRepository<Resource>
{
    public Task<IEnumerable<Resource>> GetByStateWithFiltersAsync(State state, string? search = null);
    
    public Task<Resource?> GetByTitleAsync(string title);
}
