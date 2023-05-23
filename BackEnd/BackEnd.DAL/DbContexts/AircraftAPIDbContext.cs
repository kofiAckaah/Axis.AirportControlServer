using BackEnd.DataDomain.Contracts;
using BackEnd.DataDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace BackEnd.DAL.DbContexts
{
    public class AircraftAPIDbContext : AuditableDbContext<AircraftAPIDbContext>
    {
        private readonly ICurrentUserService currentUserService;
        public AircraftAPIDbContext(DbContextOptions<AircraftAPIDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            this.currentUserService = currentUserService;
        }

        #region Entities

        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Runaway> Runaways { get; set; }
        public DbSet<AircraftLocation> AircraftLocations { get; set; }
        public DbSet<AircraftIntent> AircraftIntents { get; set; }

        #endregion

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = currentUserService.UserId;
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = currentUserService.UserId;
                        break;
                }
            }

            if (currentUserService.UserId == Guid.Empty)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }

            return await base.SaveChangesAsync(currentUserService.UserId, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
