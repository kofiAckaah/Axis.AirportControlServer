using BackEnd.DataDomain.Interfaces;

namespace BackEnd.Shared.Interfaces
{
    public interface IRepositoryAsync<T> where T : class, IEntity
    {
        IQueryable<T> GetAllEntities { get; }
        int Count { get; }

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    }
}
