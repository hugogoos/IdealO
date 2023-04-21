using Ideal.Core.Mqtt.Configurations;
using Ideal.Core.Mqtt.Configurations.Options;
using Ideal.Core.Mqtt.Options;
using Ideal.Core.Mqtt.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

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
            services.AddTransient<IConfigurationCenter, ConfigurationCenter>();
            var config = services.BuildServiceProvider().GetService<IConfigurationCenter>();
            var options = config.MqttOptions;

            return AddMqttClientSetupWithConfig(services, options);
        }

        /// <summary>
        /// Mqtt客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddMqttClientSetup(this IServiceCollection services, IEnumerable<MqttOption> options)
        {
            return AddMqttClientSetupWithConfig(services, options);
        }

        private static IServiceCollection AddMqttClientSetupWithConfig(IServiceCollection services, IEnumerable<MqttOption> options)
        {
            if (options == null || !options.Any())
            {
                throw new ArgumentNullException(nameof(options), "请检查MQTTOptions配置是否添加！");
            }

            var isRepeat = options.GroupBy(i => i.ClientId).Any(g => g.Count() > 1);
            if (isRepeat)
            {
                throw new ArgumentNullException(nameof(options), $"请确保MQTTOptions配置中[{nameof(MqttOption.ClientId)}]值唯一，不可重复！");
            }

            services.AddMqttClientSetupWithConfig(optionBuilders =>
            {
                foreach (var option in options)
                {
                    for (var i = 0; i < option.ClientCount; i++)
                    {
                        var clientId = $"{option.ClientId}.{i}.{Guid.NewGuid()}";
                        var opt = new ManagedMqttClientOptionsBuilder()
                                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                                .WithClientOptions(new MqttClientOptionsBuilder()
                                   .WithCredentials(option.User, option.Password)
                                   .WithClientId(clientId)
                                   .WithTcpServer(option.Server, option.Port)
                                   .Build()
                                ).Build();

                        optionBuilders.Add(opt);
                    }
                }
            });

            return services;
        }

        /// <summary>
        /// Mqtt客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">Mqtt配置项</param>
        /// <returns></returns>
        public static IServiceCollection AddMqttClientSetupWithConfig(this IServiceCollection services, Action<ManagedMqttClientOptionBuilder> configure)
        {
            services.AddSingleton<List<ManagedMqttClientOptions>>(serviceProvider =>
            {
                var optionBuilder = new ManagedMqttClientOptionBuilder(serviceProvider);
                configure(optionBuilder);
                return optionBuilder;
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
