using AircraftAPI.Infrastructure.Extensions;
using AircraftAPI.Infrastructure.Services;
using AircraftAPI.Shared.Interfaces;
using BackEnd.DAL.DbContexts;
using BackEnd.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.ConfigurationOptions;
using Shared.Interfaces;
using Shared.Services;
using AircraftAPI.Infrastructure.Constant;

namespace AircraftAPI.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add services to the container.
            services.AddHttpContextAccessor();
            services.AddDatabase(config);
            services.ConfigureAuth();

            services.AddControllers();
            services.ConfigureSettings(config);
            services.AddApplicationServices();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.RegisterMediatorAndMappings();

            return services;
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connStringJson = configuration.GetSection("ConnectionStrings");
            services.Configure<DbConnectionString>(connStringJson);
            services.AddOptions();
            services.AddDbContext<AircraftAPIDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        private static void ConfigureAuth(this IServiceCollection services)
        {
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddDataProtection();
        }

        private static void ConfigureSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppConfiguration>(config.GetSection(GlobalConstants.ConfigSettingsName));
            services.AddOptions();

        }
        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIntentService, IntentService>();
        }
    }
}
