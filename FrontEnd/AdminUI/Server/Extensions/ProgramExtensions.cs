using AdminUI.Infrastructure.Jobs;
using AdminUI.Infrastructure.Services;
using AdminUI.Shared.Interfcaes;
using BackEnd.DAL.DbContexts;
using BackEnd.DAL.DbSeeding;
using BackEnd.DataDomain.Entities;
using BackEnd.Infrastructure.Services;
using BackEnd.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Shared.ConfigurationOptions;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Services;
using static Quartz.Logging.OperationName;

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

            services.ConfigureQuartzAsync();
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
            
            app.UseAuthentication();
            app.UseAuthorization();


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
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            services.AddDataProtection();
        }

        private static void AddBackEndServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGroundCrewService, GroundCrewService>();

            services.AddScoped<IAdminManager, AdminManager>();
        }

        private static async Task ConfigureQuartzAsync(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = new JobKey(nameof(LandingJob));
                q.AddJob<LandingJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("landing-trigger")
                    .WithCronSchedule("0 0/1 * 1/1 * ? *"));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            services.AddTransient<LandingJob>();
            var container = services.BuildServiceProvider();

            // Create an instance of the job factory
            var jobFactory = new JobFactory(container);

            // Create a Quartz.NET scheduler
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();

            // Tell the scheduler to use the custom job factory
            scheduler.JobFactory = jobFactory;
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
