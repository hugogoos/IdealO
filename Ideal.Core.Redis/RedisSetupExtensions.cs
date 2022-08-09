using CSRedis;
using Ideal.Core.Redis.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ideal.Core.Redis
{
    public static class RedisSetupExtensions
    {
        /// <summary>
        /// Redis客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisClientSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigManager, ConfigManager>();
            var config = services.BuildServiceProvider().GetService<IConfigManager>();
            var options = config.RedisOptions;
            if (options == null)
            {
                throw new ArgumentException("请检查RedisOptions配置是否添加");
            }

            services.AddSingleton(serviceProvider =>
            {
                var csredis = new CSRedisClient($"{options.Default.Server}:{options.Default.Port},password={options.Default.Password},defaultDatabase={options.Default.DefaultDatabase}");
                RedisHelper.Initialization(csredis);
                return csredis;
            });

            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }
    }
}
