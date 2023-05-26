﻿using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System.Collections.Concurrent;

namespace AdminUI.Infrastructure.Jobs
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;
        protected readonly ConcurrentDictionary<IJob, IServiceScope> scopes = new ConcurrentDictionary<IJob, IServiceScope>();

        public JobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = serviceProvider.CreateScope();
            IJob job;

            try
            {
                job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
            if (!scopes.TryAdd(job, scope))
            {
                scope.Dispose();
                throw new Exception("Failed to track DI scope");
            }

            return job;
        }

        public void ReturnJob(IJob job)
        {
            if (scopes.TryRemove(job, out var scope))
            {
                scope.Dispose();
            }
        }
    }
}
