using BackEnd.DataDomain.Contracts;

namespace BackEnd.DataDomain.Entities
{
    public class Runaway : AuditableEntity
    {
        public bool IsFree { get; set; }
    }
}
