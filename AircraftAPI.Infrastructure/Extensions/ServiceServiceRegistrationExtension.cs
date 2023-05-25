using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AircraftAPI.Infrastructure.Extensions
{
    public static class ServiceServiceRegistrationExtension
    {
        public static void RegisterMediatorAndMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }
    }
}
