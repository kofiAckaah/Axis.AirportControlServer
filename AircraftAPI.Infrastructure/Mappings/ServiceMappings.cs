using AircraftAPI.Infrastructure.Commands;
using AutoMapper;
using BackEnd.DataDomain.Entities;

namespace AircraftAPI.Infrastructure.Mappings
{
    public class ServiceMappings : Profile
    {
        public ServiceMappings()
        {
            CreateMap<SaveLocationCommand, AircraftLocation>().ReverseMap();
        }
    }
}
