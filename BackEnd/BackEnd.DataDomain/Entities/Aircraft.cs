using BackEnd.DataDomain.Contracts;

namespace BackEnd.DataDomain.Entities
{
    public class Aircraft : AuditableEntity
    {
        public string AircraftType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public int Heading { get; set; }

        public virtual ICollection<AircraftIntent> AircraftIntents { get; set; }
    }
}
