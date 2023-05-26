using AdminUI.Shared.Interfcaes;
using BackEnd.DataDomain.Entities;
using BackEnd.Shared.Interfaces;

namespace AdminUI.Infrastructure.Services
{
    public class GroundCrewService : IGroundCrewService
    {
        private readonly IUnitOfWork unitOfWork;

        public GroundCrewService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task ParkAirplane()
        {
            var runaway = unitOfWork.Repository<Runaway>().GetAllEntities.FirstOrDefault();
            if (runaway is not { IsFree: false })
                return;

            runaway.IsFree = true;
            await unitOfWork.Repository<Runaway>().UpdateAsync(runaway);
            await unitOfWork.Commit(CancellationToken.None);
        }
    }
}
