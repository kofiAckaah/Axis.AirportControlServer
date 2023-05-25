using AircraftAPI.Infrastructure.Commands;
using AircraftAPI.Infrastructure.Requests;
using AutoMapper;
using BackEnd.DataDomain.Entities;

namespace AircraftAPI.Infrastructure.Mappings
{
    public class ServiceMappings : Profile
    {
        public ServiceMappings()
        {
            CreateMap<SaveLocationCommand, AircraftLocationRequest>().ReverseMap();
            CreateMap<AircraftLocation, SaveLocationCommand>().ReverseMap();
        }
    }
}
