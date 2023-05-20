using System.ComponentModel.DataAnnotations.Schema;
using BackEnd.DataDomain.Entities;

namespace BackEnd.DataDomain.Contracts
{
    public class AircraftRank : AuditableEntity
    {
        [ForeignKey(nameof(Aircraft))]
        public Guid AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
    }
}
