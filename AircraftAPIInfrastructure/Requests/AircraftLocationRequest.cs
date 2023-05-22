using AircraftAPI.Shared.Interfaces;

namespace Aircraft.APIInfrastructure.Requests
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
