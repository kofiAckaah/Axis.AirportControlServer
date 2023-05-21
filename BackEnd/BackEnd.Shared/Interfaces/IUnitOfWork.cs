using BackEnd.DataDomain.Interfaces;

namespace BackEnd.Shared.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit(CancellationToken cancellationToken);
        IRepositoryAsync<T> Repository<T>() where T : class, IEntity;
    }
}
