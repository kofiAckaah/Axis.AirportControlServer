using BackEnd.DataDomain.Contracts;
using BackEnd.DataDomain.Enum;

namespace BackEnd.DataDomain.Entities
{
    public class AircraftIntent : AircraftRank
    {
        public IntentType IntentType { get; set; }
    }
}
