using BackEnd.DataDomain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.DAL.DbContexts
{
    public class BaseDbContext<T> : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> where T : DbContext
    {

        protected BaseDbContext(DbContextOptions<T> options) : base(options)
        {
        }

        public DbSet<Aircraft> Aircrafts { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
