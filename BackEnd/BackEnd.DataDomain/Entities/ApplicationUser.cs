using BackEnd.DataDomain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.DataDomain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
