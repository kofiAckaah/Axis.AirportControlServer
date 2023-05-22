namespace AircraftAPI.Shared.Interfaces
{
    public interface IAircraftLocationRequest
    {
        public string AircraftType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public int Heading { get; set; }
    }
}
