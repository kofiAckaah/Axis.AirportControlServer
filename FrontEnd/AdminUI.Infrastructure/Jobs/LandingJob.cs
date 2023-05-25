using BackEnd.DataDomain.Entities;
using BackEnd.Shared.Interfaces;
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
            var work = serviceProvider.GetRequiredService<IUnitOfWork>();
            var run = work.Repository<Runaway>().GetAllEntities;
            return Task.CompletedTask;
        }
    }
}
