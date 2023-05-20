using BackEnd.DataDomain.Enum;
using BackEnd.DataDomain.Interfaces;

namespace BackEnd.DataDomain.Entities.Audits
{
    public class Audit : IEntity
    {
        public Guid Id { get; set; }
        public AuditType AuditType { get; set; }
        public Guid UserId { get; set; }
        public string TableName { get; set; }
        public DateTime AuditDateTime { get; set; }
        public string? OldEntry { get; set; }
        public string NewEntry { get; set; }
        public string? AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
    }
}
