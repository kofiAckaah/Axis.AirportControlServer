using BackEnd.DataDomain.Contracts;

namespace BackEnd.DataDomain.Entities
{
    public class ParkingSpot : AuditableEntity
    {
        public bool IsEmpty { get; set; }
    }
}
