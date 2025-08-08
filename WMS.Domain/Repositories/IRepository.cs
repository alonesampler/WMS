namespace WMS.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    public Task<TEntity?> GetByIdAsync(Guid id);

    public Task CreateAsync(TEntity entity);

    public Task UpdateAsync(TEntity entity);

    public Task DeleteAsync(TEntity entity);
}
