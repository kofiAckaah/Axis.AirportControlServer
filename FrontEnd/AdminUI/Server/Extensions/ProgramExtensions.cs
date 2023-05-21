using AdminUI.Server.Services;
using BackEnd.DAL.DbContexts;
using BackEnd.DataDomain.Entities;
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


            return app;
        }

        private static void AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connStringJson = configuration.GetSection("ConnectionStrings");
            services.Configure<DbConnectionString>(connStringJson);
            services.AddOptions();
            services.AddDbContext<ControlServerDbContext>(options 
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
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


        private static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz();
        }

    }
}
