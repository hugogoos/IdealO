using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Ideal.Core.Quartz
{
    public class JobFactory : IJobFactory
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 实现接口Job
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                using var _scope = _serviceProvider.CreateScope();
                var job = _scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
