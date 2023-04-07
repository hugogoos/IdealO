using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ideal.Core.SignalR
{
    public static class SignalRSetupExtensions
    {
        /// <summary>
        /// SignalR配置项
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="url">SignalR服务器地址</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSignalRSetup(this IServiceCollection services, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("请检查url配置是否添加！");
            }

            services.AddSingleton(sp =>
            {
                var connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();
                return connection;
            });

            services.AddSingleton<SignalRRepository>();
            services.AddSingleton<IHostedService>(serviceProvider =>
            {
                return serviceProvider.GetService<SignalRRepository>();
            });

            services.AddSingleton(serviceProvider =>
            {
                var signalRRepository = serviceProvider.GetService<SignalRRepository>();
                var signalRRepositoryProvider = new SignalRRepositoryProvider(signalRRepository);
                return signalRRepositoryProvider;
            });
            services.AddSingleton<ISignalRService, SignalRService>();
        }
    }
}
