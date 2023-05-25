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

        public async Task<bool> CanApproachAsync(CancellationToken token = default)
        {
            var freeRunaway = unitOfWork.Repository<Runaway>().GetAllEntities.FirstOrDefault();
            if (freeRunaway is not { IsFree: true })
                return false;

            var parkingSpot = unitOfWork.Repository<ParkingSpot>().GetAllEntities
                                        .FirstOrDefault(p => p.IsEmpty);
            if (parkingSpot is not { IsEmpty: true })
                return false;

            var rResult = await SetRunawayStatusAsync(false, true, freeRunaway);
            var pResult = await SetParkingSpotStatusAsync(false, true, parkingSpot);

            var result = rResult && pResult;
            if (result)
                await CommitAsync(token);

            return result;
        }

        private async Task CommitAsync(CancellationToken token = default) => await unitOfWork.Commit(token);

        public bool CanPark()
        {
            var parkingSpot = unitOfWork.Repository<ParkingSpot>().GetAllEntities.FirstOrDefault(p => p.IsEmpty);
            return parkingSpot is { IsEmpty: true };
        }

        public async Task<bool> CanTakeOff(CancellationToken token)
        {
            var runaway = unitOfWork.Repository<Runaway>().GetAllEntities.FirstOrDefault(r => r.IsFree);
            var result = runaway is { IsFree: true };
            if (!result)
                return false;

            runaway.IsFree = false;
            await unitOfWork.Repository<Runaway>().UpdateAsync(runaway);
            await CommitAsync(token);

            return true;
        }

        public async Task CancelTakeOff(CancellationToken token)
        {
            await SetRunawayStatusAsync(true, false);
            await CommitAsync(token);
        }

        public async Task<bool> TakeOff(CancellationToken token = default)
        {
            var rRunaway = await SetRunawayStatusAsync(true, false);
            var rSpot = await SetParkingSpotStatusAsync(true, false);

            if (rSpot && rRunaway)
                await CommitAsync(token);

            return rSpot && rRunaway;
        }

        public async Task<bool> SetParkingSpotStatusAsync(bool freeSpot,
                                                          bool check = true,
                                                          ParkingSpot? parkingSpot = null)
        {
            parkingSpot ??= unitOfWork.Repository<ParkingSpot>().GetAllEntities.FirstOrDefault(p => p.IsEmpty == check);
            if (parkingSpot == null || parkingSpot.IsEmpty == freeSpot)
                return false;

            parkingSpot.IsEmpty = freeSpot;
            await unitOfWork.Repository<ParkingSpot>().UpdateAsync(parkingSpot);
            return true;
        }

        public async Task<bool> SetRunawayStatusAsync(bool freeRunaway,
                                                      bool check = true,
                                                      Runaway? runaway = null)
        {
            runaway ??= unitOfWork.Repository<Runaway>().GetAllEntities.FirstOrDefault(r => r.IsFree == check);
            if (runaway == null || runaway.IsFree == freeRunaway)
                return false;

            runaway.IsFree = freeRunaway;
            await unitOfWork.Repository<Runaway>().UpdateAsync(runaway);

            return true;
        }

        public async Task CancelApproachAsync(CancellationToken token = default)
        {
            var rResult = await SetRunawayStatusAsync(true, false);
            var pResult = await SetParkingSpotStatusAsync(true, false);

            if (rResult && pResult)
                await CommitAsync(token);
        }
    }
}
