using BackEnd.DataDomain.Entities.Audits;
using BackEnd.DataDomain.Enum;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.DAL.DbContexts
{
    public class AuditableDbContext<T> : BaseDbContext<T> where T : DbContext
    {
        protected AuditableDbContext(DbContextOptions<T> options) : base(options)
        {
        }

        public DbSet<Audit> AuditRecords { get; set; }

        public virtual async Task<int> SaveChangesAsync(Guid userId = default, CancellationToken cancellationToken = default)
        {
            try
            {
                var auditEntries = OnBeforeSaveChanges(userId);
                var result = await base.SaveChangesAsync(cancellationToken);
                await OnAfterSaveChanges(auditEntries);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AuditEntry> OnBeforeSaveChanges(Guid userId)
        {
            try
            {
                ChangeTracker.DetectChanges();
                var auditEntries = new List<AuditEntry>();
                foreach (var entry in ChangeTracker.Entries())
                {
                    if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                        continue;

                    var auditEntry = new AuditEntry(entry)
                    {
                        TableName = entry.Entity.GetType().Name,
                        UserId = userId
                    };
                    auditEntries.Add(auditEntry);
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsTemporary)
                        {
                            auditEntry.TemporaryProperties.Add(prop);
                            continue;
                        }

                        var propName = prop.Metadata.Name;
                        if (prop.Metadata.IsPrimaryKey())
                        {
                            auditEntry.NewEntries[propName] = prop.CurrentValue;
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                auditEntry.AuditType = AuditType.Create;
                                auditEntry.NewEntries[propName] = prop.CurrentValue;
                                break;
                            case EntityState.Deleted:
                                auditEntry.AuditType = AuditType.Delete;
                                auditEntry.OldEntries[propName] = prop.OriginalValue;
                                break;
                            case EntityState.Modified:
                                if (prop.IsModified)
                                {
                                    auditEntry.AuditType = AuditType.Update;
                                    auditEntry.ChangedColumns.Add(propName);
                                    auditEntry.NewEntries[propName] = prop.CurrentValue;
                                    auditEntry.OldEntries[propName] = prop.OriginalValue;
                                }
                                break;
                        }
                    }
                }

                foreach (var entry in auditEntries.Where(_ => !_.HasTemporyProperties))
                {
                    AuditRecords.Add(entry.AddAudit());
                }

                return auditEntries.Where(_ => _.HasTemporyProperties).ToList();
            }
            catch (Exception ex)
            {
                //TODO: log here
                throw;
            }
        }

        public Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            try
            {
                if (auditEntries == null || !auditEntries.Any())
                    return Task.CompletedTask;

                foreach (var entry in auditEntries)
                {
                    foreach (var prop in entry.TemporaryProperties)
                    {
                        if (prop.Metadata.IsPrimaryKey())
                            entry.KeyEntries[prop.Metadata.Name] = prop.CurrentValue;
                        else
                            entry.NewEntries[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    AuditRecords.Add(entry.AddAudit());
                }

                return SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
