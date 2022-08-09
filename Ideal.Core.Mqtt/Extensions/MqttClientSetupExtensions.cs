using Ideal.Core.Mqtt.Configurations;
using Ideal.Core.Mqtt.Options;
using Ideal.Core.Mqtt.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Ideal.Core.Mqtt.Extensions
{
    /// <summary>
    /// Mqtt客户端启动项
    /// </summary>
    public static class MqttClientSetupExtensions
    {
        /// <summary>
        /// Mqtt客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMqttClientSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigManager, ConfigManager>();
            var config = services.BuildServiceProvider().GetService<IConfigManager>();
            var options = config.MQTTOptions;
            if (options == null)
            {
                throw new ArgumentNullException("请检查MQTTOptions配置是否添加！");
            }

            services.AddMqttClientSetupWithConfig(optionBuilder =>
            {
                optionBuilder
                .WithCredentials(options.User, options.Password)
                .WithClientId($"{options.ClientId}.{Guid.NewGuid()}")
                .WithTcpServer(options.Server, options.Port);
            });
            return services;
        }

        /// <summary>
        /// Mqtt客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">Mqtt配置项</param>
        /// <returns></returns>
        public static IServiceCollection AddMqttClientSetupWithConfig(this IServiceCollection services, Action<MqttClientOptionBuilder> configure)
        {
            services.AddSingleton(serviceProvider =>
            {
                var optionBuilder = new MqttClientOptionBuilder(serviceProvider);
                configure(optionBuilder);
                return optionBuilder.Build();
            });
            services.AddSingleton<MqttClientRepository>();
            services.AddSingleton<IHostedService>(serviceProvider =>
            {
                return serviceProvider.GetService<MqttClientRepository>();
            });
            services.AddSingleton(serviceProvider =>
            {
                var mqttClientRepository = serviceProvider.GetService<MqttClientRepository>();
                var mqttClientRepositoryProvider = new MqttClientRepositoryProvider(mqttClientRepository);
                return mqttClientRepositoryProvider;
            });
            services.AddSingleton<IMqttClientService, MqttClientService>();
            return services;
        }
    }
}
