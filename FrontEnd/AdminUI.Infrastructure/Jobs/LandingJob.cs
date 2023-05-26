using AdminUI.Shared.Interfcaes;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AdminUI.Infrastructure.Jobs
{
    public class LandingJob : IJob
    {
        private readonly IServiceProvider serviceProvider;
        public LandingJob(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var groundCrew = serviceProvider.GetRequiredService<IGroundCrewService>();
            groundCrew.ParkAirplane();
            return Task.CompletedTask;
        }
    }
}
