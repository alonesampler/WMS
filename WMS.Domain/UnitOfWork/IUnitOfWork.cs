using WMS.Domain.Repositories;

namespace WMS.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public IResourceRepository ResourceRepository { get; }
    public IUnitOfMeasureRepository UnitOfMeasureRepository { get; }
    public IReceiptDocumentRepository ReceiptDocumentRepository { get; }

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollBackAsync();
}