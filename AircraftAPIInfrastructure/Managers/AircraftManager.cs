using System.Net;
using AircraftAPI;
using AircraftAPI.Shared.Constants;
using AircraftAPI.Shared.Interfaces;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Aircraft.APIInfrastructure.Requests;

namespace AircraftAPIInfrastructure.Managers
{
    public class AircraftManager : IAircraftManager
    {
        private readonly HttpClient httpClient;

        public AircraftManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetWeatherAsync()
        {
            var response = await httpClient.GetFromJsonAsync<WeatherForecast>(AircraftEndpoints.WeatherEndpoint);

            return response;
        }

        public async Task<HttpStatusCode> SendLocation(AircraftLocationRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(AircraftEndpoints.SendLocation, request);
            return response.StatusCode;
        }
    }
}
