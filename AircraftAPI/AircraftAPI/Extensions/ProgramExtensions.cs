using AircraftAPI.Infrastructure.Services;
using AircraftAPI.Shared.Interfaces;
using BackEnd.Infrastructure.Services;
using BackEnd.Shared.Interfaces;

namespace AircraftAPI.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIntentService, IntentService>();

            //add mediatr
            //add mappings
        }
    }
}
