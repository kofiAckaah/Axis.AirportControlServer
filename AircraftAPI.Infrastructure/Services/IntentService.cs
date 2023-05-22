using AircraftAPI.Shared.Interfaces;
using BackEnd.DataDomain.Entities;
using BackEnd.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Shared.ConfigurationOptions;

namespace AircraftAPI.Infrastructure.Services
{
    public class IntentService : IIntentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AppConfiguration appConfig;

        public IntentService(IUnitOfWork unitOfWork, IOptions<AppConfiguration> appConfig)
        {
            this.unitOfWork = unitOfWork;
            this.appConfig = appConfig.Value;
        }

        public bool CanApproach()
        {
            var freeRunaway = unitOfWork.Repository<Runaway>().GetAllEntities.First().IsFree;
            if (!freeRunaway)
                return freeRunaway;

            var emptyParkingSpot = unitOfWork.Repository<ParkingSpot>().GetAllEntities
                                        .Any(p => p.IsEmpty);

            return freeRunaway && emptyParkingSpot;
        }
    }
}
