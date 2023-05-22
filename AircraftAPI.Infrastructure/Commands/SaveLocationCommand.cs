using AircraftAPI.Shared.Interfaces;
using AutoMapper;
using BackEnd.DataDomain.Entities;
using BackEnd.Shared.Interfaces;
using MediatR;

namespace AircraftAPI.Infrastructure.Commands
{
    public class SaveLocationCommand : IRequest<bool>, IAircraftLocationRequest
    {
        public string AircraftType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public int Heading { get; set; }

        public string CallSign { get; set; }
    }

    public class SaveLocationCommandHandler : IRequestHandler<SaveLocationCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SaveLocationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(SaveLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var locs = mapper.Map<AircraftLocation>(request);
                await unitOfWork.Repository<AircraftLocation>().AddAsync(locs);

                await unitOfWork.Commit(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
