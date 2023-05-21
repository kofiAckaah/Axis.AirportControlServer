using AdminUI.Server.Services;
using BackEnd.DAL.DbContexts;
using BackEnd.DAL.DbSeeding;
using BackEnd.DataDomain.Entities;
using BackEnd.Infrastructure.Services;
using BackEnd.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Shared.ConfigurationOptions;
using Shared.Extensions;
using Shared.Interfaces;

namespace AdminUI.Server.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection ConfigureService(this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            // Add services to the container.
            services.AddDatabase(configurationManager);
            services.AddIdentity();
            services.ConfigureAuth();
            services.GetApplicationSettings(configurationManager);

            services.AddBackEndServices();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.ConfigureQuartz();
            return services;
        }

        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Initialize();


            return app;
        }

        private static void AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connStringJson = configuration.GetSection("ConnectionStrings");
            services.Configure<DbConnectionString>(connStringJson);
            services.AddOptions();
            services.AddDbContext<ControlServerDbContext>(options 
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ISeeder, Seeder>();
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services
                //.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                //.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                //application wide dbcontext
                .AddIdentity<ApplicationUser, ApplicationRole>(options => options.ConfigureIdentityOptions())
                .AddEntityFrameworkStores<ControlServerDbContext>()
                .AddDefaultTokenProviders();
        }

        private static void ConfigureAuth(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddDataProtection();
        }

        private static void AddBackEndServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz();
        }

        private static AppConfiguration GetApplicationSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppConfiguration>();
        }

        #region Configure Application
        
        private static void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var initializers = serviceScope.ServiceProvider.GetServices<ISeeder>();

            foreach (var initializer in initializers)
            {
                initializer.Seed();
            }
        }

        #endregion

    }
}
