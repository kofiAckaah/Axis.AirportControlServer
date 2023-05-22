using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Aircraft.APIInfrastructure.Extensions
{
    public static class AircraftExtensions
    {
        public static string RetrieveCallSign(this IHeaderDictionary headers)
        {
            var callSign = headers.TryGetValue("call_sign", out var vals);
            return callSign ? vals.First() : string.Empty;
        }
        public static string RetrieveCallSign(this HttpRequest headers)
        {
            var callSign = headers.Headers.TryGetValue("call_sign", out var vals);
            return callSign ? vals.First() : string.Empty;
        }
    }
}
