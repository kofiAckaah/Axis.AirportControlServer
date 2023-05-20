using BackEnd.DataDomain.Enum;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace BackEnd.DataDomain.Entities.Audits
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entityEntry)
        {
            EntityEntry = entityEntry;
        }

        public EntityEntry EntityEntry { get; }
        public Guid UserId { get; set; }
        public string TableName { get; set; }
        public AuditType AuditType { get; set; }
        public Dictionary<string, object> KeyEntries { get; } = new();
        public Dictionary<string, object> OldEntries { get; } = new();
        public Dictionary<string, object> NewEntries { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();
        public List<string> ChangedColumns { get; } = new();
        public bool HasTemporyProperties => TemporaryProperties.Any();

        public Audit AddAudit() => new()
        {
            UserId = UserId,
            TableName = TableName,
            AuditType = AuditType,
            AuditDateTime = DateTime.UtcNow,
            PrimaryKey = JsonConvert.SerializeObject(KeyEntries),
            OldEntry = OldEntries.Count == 0 ? null : JsonConvert.SerializeObject(OldEntries),
            NewEntry = NewEntries.Count == 0 ? null : JsonConvert.SerializeObject(NewEntries),
            AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns)
        };
    }
}
