using BackEnd.DAL.DbContexts;
using BackEnd.DataDomain.Interfaces;
using BackEnd.Shared.Interfaces;
using System.Collections;

namespace AircraftAPI.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AircraftAPIDbContext appDbContext;
        private Hashtable repos;

        public UnitOfWork(AircraftAPIDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IRepositoryAsync<T> Repository<T>() where T : class, IEntity
        {
            repos ??= new();
            var type = typeof(T).Name;

            if (!repos.ContainsKey(type))
            {
                var repoType = typeof(RepositoryAsync<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), appDbContext);
                repos.Add(type, repoInstance);
            }

            return (IRepositoryAsync<T>)repos[type];
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await appDbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    appDbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
