using AircraftAPI.Shared.Enums;
using AircraftAPI.Shared.Interfaces;
using BackEnd.DataDomain.Enum;
using BackEnd.Shared.Interfaces;
using MediatR;

namespace AircraftAPI.Infrastructure.Queries
{
    public class AircraftIntentQuery : IRequest<ApiResponseType>
    {
        public string CallSign { get; set; }
        public IntentType Intent { get; set; }
    }

    public class AircraftIntentQueryHandler : IRequestHandler<AircraftIntentQuery, ApiResponseType>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IIntentService intentService;

        public AircraftIntentQueryHandler(IUnitOfWork unitOfWork, IIntentService intentService)
        {
            this.unitOfWork = unitOfWork;
            this.intentService = intentService;
        }

        public async Task<ApiResponseType> Handle(AircraftIntentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.Intent)
                {
                    case IntentType.Approach:
                        return await intentService.CanApproachAsync(cancellationToken) ? ApiResponseType.Success : ApiResponseType.Failure;
                    case IntentType.Parking:
                        break;
                    case IntentType.Cancel:
                        await intentService.CancelApproachAsync(cancellationToken);
                        return ApiResponseType.Success;
                    case IntentType.TakeOff:
                        return await intentService.TakeOff(cancellationToken) ? ApiResponseType.Success : ApiResponseType.Failure;
                    case IntentType.CanTakeOff:
                        return await intentService.CanTakeOff(cancellationToken) ? ApiResponseType.Success : ApiResponseType.Failure;
                    case IntentType.CancelTakeOff:
                        await intentService.CancelTakeOff(cancellationToken);
                        return ApiResponseType.Success;
                }

                return ApiResponseType.Failure;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ApiResponseType.Failure;
            }
        }
    }
}
