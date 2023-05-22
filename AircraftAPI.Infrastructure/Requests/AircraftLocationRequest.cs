using AircraftAPI.Shared.Interfaces;

namespace AircraftAPI.Infrastructure.Requests
{
    public class AircraftLocationRequest : IAircraftLocationRequest
    {
        public string AircraftType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public int Heading { get; set; }
    }
}
