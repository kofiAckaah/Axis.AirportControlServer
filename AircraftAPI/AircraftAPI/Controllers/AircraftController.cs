using AircraftAPI.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AircraftAPI.Infrastructure.Attributes;
using AircraftAPI.Infrastructure.Commands;
using AircraftAPI.Infrastructure.Extensions;
using AircraftAPI.Infrastructure.Queries;
using AircraftAPI.Infrastructure.Requests;
using AutoMapper;
using BackEnd.DataDomain.Enum;
using MediatR;

namespace AircraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [VerifyPublicKey]
    public class AircraftController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly HttpContext context;

        private string callSign;

        public AircraftController(IMediator mediator, IMapper mapper, IHttpContextAccessor context)
        {
            this.context = context.HttpContext;
            this.mediator = mediator;
            this.mapper = mapper;

        }

        [HttpPost(AircraftEndpoints.SendLocation)]
        public async Task<HttpStatusCode> SendLocation(AircraftLocationRequest request)
        {
            callSign = context.Request.Headers.RetrieveCallSign();
            if (string.IsNullOrEmpty(callSign))
                return HttpStatusCode.Conflict;

            var command = mapper.Map<SaveLocationCommand>(request);
            command.CallSign = callSign;
            if (await mediator.Send(command))
                return HttpStatusCode.NoContent;

            return HttpStatusCode.Conflict;
        }

        [HttpPost($"{AircraftEndpoints.Intents}/{{intent}}")]
        public async Task<HttpStatusCode> SendIntent(string intent)
        {
            callSign = context.Request.Headers.RetrieveCallSign();
            if (string.IsNullOrEmpty(callSign))
                return HttpStatusCode.Conflict;

            var passed = Enum.TryParse(intent, true, out IntentType intentType);
            if (!passed)
            {
                return HttpStatusCode.Conflict;
            }
            var result = await mediator.Send(new AircraftIntentQuery()
            {
                CallSign = callSign,
                Intent = intentType
            });

            return result.ToStatusCode();
        }
    }
}
