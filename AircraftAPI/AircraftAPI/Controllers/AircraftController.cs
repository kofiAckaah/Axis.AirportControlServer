using AircraftAPI.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    public class AircraftController : ControllerBase
    {
        private readonly HttpContextAccessor contextAccessor;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly HttpContext context;

        private string callSign;

        public AircraftController(IMediator mediator, IMapper mapper)
        {
            context = HttpContext;
            this.mediator = mediator;
            this.mapper = mapper;

            callSign = context.Request.Headers.RetrieveCallSign();
        }

        [HttpGet(Name = AircraftEndpoints.SendLocation)]
        public async Task<HttpStatusCode> SendLocation(AircraftLocationRequest request)
        {
            var command = mapper.Map<SaveLocationCommand>(request);
            command.CallSign = callSign;
            if (await mediator.Send(command))
                return HttpStatusCode.NoContent;

            return HttpStatusCode.Conflict;
        }

        //[HttpPost($"{AircraftEndpoints.Intents}/{{intent}}")]
        [HttpPost("intent/{intent}")]
        public async Task<HttpStatusCode> SendIntent(string intent)
        {
            var passed = Enum.TryParse(intent, out IntentType intentType);
            if (passed)
            {
                var result = await mediator.Send(new AircraftIntentQuery()
                {
                    CallSign = callSign, 
                    Intent = intentType
                });

                return result.ToStatusCode();
            }
            return HttpStatusCode.Conflict;
        }
    }
}
