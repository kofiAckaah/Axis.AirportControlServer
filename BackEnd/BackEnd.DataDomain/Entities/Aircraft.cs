using BackEnd.DataDomain.Contracts;

namespace BackEnd.DataDomain.Entities
{
    public class Aircraft : AuditableEntity
    {
        public string CallSign { get; set; }

        public virtual ICollection<AircraftIntent> AircraftIntents { get; set; }
    }
}
