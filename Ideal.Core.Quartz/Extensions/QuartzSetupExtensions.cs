using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Ideal.Core.Quartz.Extensions
{
    /// <summary>
    /// Quartz启动项
    /// </summary>
    public static class QuartzSetupExtensions
    {
        /// <summary>
        /// Quartz启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzSetup(this IServiceCollection services)
        {
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();
            return services;
        }
    }
}
