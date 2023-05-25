using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using AircraftAPI.Shared.Enums;
using BackEnd.DataDomain.Enum;

//using Microsoft.AspNetCore.Http;

namespace AircraftAPI.Infrastructure.Extensions
{
    public static class AircraftExtensions
    {
        public static string? RetrieveCallSign(this IHeaderDictionary headers)
        {
            if (!headers.Any())
                return string.Empty;

            var callSign = headers.TryGetValue("call_sign", out var vals);
            return callSign ? vals.ToList().First() : string.Empty;
        }

        public static HttpStatusCode ToStatusCode(this ApiResponseType intentType)
        {
            return intentType switch
            {
                ApiResponseType.Success => HttpStatusCode.NoContent,
                ApiResponseType.Unauthorized => HttpStatusCode.Unauthorized,
                ApiResponseType.BadRequest => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.Conflict,
            };
        }
    }
}
