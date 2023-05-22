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
        public static string RetrieveCallSign(this IHeaderDictionary headers)
        {
            if (headers.Any())
                return string.Empty;

            var callSign = headers.TryGetValue("call_sign", out var vals);
            return callSign ? vals.ToList().First() : string.Empty;
        }

        public static HttpStatusCode ToStatusCode(this ApiResponseType intentType)
        {
            switch (intentType)
            {
                case ApiResponseType.Success:
                    return HttpStatusCode.NoContent;
                case ApiResponseType.Unauthorized:
                    return HttpStatusCode.Unauthorized;
                case ApiResponseType.BadRequest:
                    return HttpStatusCode.BadRequest;
                case ApiResponseType.Failure:
                default:
                    return HttpStatusCode.Conflict;
            }
        }
    }
}
