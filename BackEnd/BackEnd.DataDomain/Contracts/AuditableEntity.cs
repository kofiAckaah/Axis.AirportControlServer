using BackEnd.DataDomain.Interfaces;

namespace BackEnd.DataDomain.Contracts
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
