using Microsoft.EntityFrameworkCore.Storage;
using WMS.Domain.Repositories;
using WMS.Domain.UnitOfWork;
using WMS.Infrastructure.Persistens;

namespace WMS.Infrastructure;

public class PostgresUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction _transaction;
    private readonly IResourceRepository _resourceRepository;
    private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;

    public PostgresUnitOfWork(
        ApplicationDbContext dbContext,
        IResourceRepository resourceRepository,
        IUnitOfMeasureRepository unitOfMeasureRepository)
    {
        _dbContext = dbContext;
        _resourceRepository = resourceRepository;
        _unitOfMeasureRepository = unitOfMeasureRepository;
    }
    
    public IResourceRepository ResourceRepository => _resourceRepository;
    public IUnitOfMeasureRepository UnitOfMeasureRepository => _unitOfMeasureRepository;
    
    public async Task BeginTransactionAsync()
    {
        if(_transaction != null)
            throw new InvalidOperationException("Transaction already started!");

        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }
    
    public async Task CommitAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            await _transaction?.CommitAsync();
        }
        catch (Exception)
        {
            await RollBackAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollBackAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction must be in progress");

        try
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    
    public void Dispose() => 
        _dbContext.Dispose();
}