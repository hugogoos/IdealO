using Ideal.Core.Orm.SqlSugar.Configurations;
using Ideal.Core.Orm.SqlSugar.Options;
using Ideal.Core.Orm.SqlSugar.Organization;
using Ideal.Core.Orm.SqlSugar.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Linq;

namespace Ideal.Core.Orm.SqlSugar.Extensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlSugarSetupExtensions
    {
        /// <summary>
        /// SqlSugar启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">数据库字符串链接</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSqlSugarSetup(this IServiceCollection services, string connectionString)
        {
            // 把多个连接对象注入服务，这里必须采用Scope，因为有事务操作
            services.AddScoped<ISqlSugarClient>(o =>
            {
                return new SqlSugarScope(new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true,
                        DefaultCacheDurationInSeconds = 5,
                        IsAutoRemoveDataCache = true
                    },
                    //ConfigureExternalServices = new ConfigureExternalServices
                    //{
                    //    DataInfoCacheService = new SugarMemoryCache()
                    //}
                });
            });

            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }

        /// <summary>
        /// SqlSugar启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigManager, ConfigManager>();
            var config = services.BuildServiceProvider().GetService<IConfigManager>();
            var connectionString = config.ConnectionString;
            if (connectionString == null)
            {
                throw new ArgumentException("请检查ConnectionString配置是否添加");
            }

            services.AddSqlSugarSetupWithConfig(optionBuilder =>
            {
                optionBuilder.ConnectionString = connectionString;
                optionBuilder.DbType = DbType.MySql;
                optionBuilder.IsAutoCloseConnection = true;
                optionBuilder.InitKeyType = InitKeyType.Attribute;
                optionBuilder.MoreSettings = new ConnMoreSettings()
                {
                    IsWithNoLockQuery = true,
                    DefaultCacheDurationInSeconds = 5,
                    IsAutoRemoveDataCache = true
                };
            });
            return services;
        }

        /// <summary>
        /// SqlSugar 主从启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugaMasterSlaverSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigManager, ConfigManager>();
            var config = services.BuildServiceProvider().GetService<IConfigManager>();
            var options = config.ConnectionStrings;
            if (options == null)
            {
                throw new ArgumentException(nameof(config.ConnectionString));
            }

            services.AddSqlSugarSetupWithConfig(optionBuilder =>
            {
                optionBuilder.ConnectionString = options.Master;
                optionBuilder.DbType = DbType.MySql;
                optionBuilder.IsAutoCloseConnection = true;
                optionBuilder.InitKeyType = InitKeyType.Attribute;
                optionBuilder.MoreSettings = new ConnMoreSettings()
                {
                    IsWithNoLockQuery = true,
                    DefaultCacheDurationInSeconds = 5,
                    IsAutoRemoveDataCache = true
                };

                optionBuilder.SlaveConnectionConfigs = options.Slaves.Select(connectionString => new SlaveConnectionConfig
                {
                    HitRate = 10,
                    ConnectionString = connectionString
                }).ToList();
            });
            return services;
        }

        /// <summary>
        /// Mqtt客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">Mqtt配置项</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarSetupWithConfig(this IServiceCollection services, Action<ConnectionConfigOptions> configure)
        {
            services.AddScoped<OrgContext>();

            // 把多个连接对象注入服务，这里必须采用Scope，因为有事务操作
            services.AddSingleton<ISqlSugarClient>(serviceProvider =>
            {
                var optionBuilder = new ConnectionConfigOptions(serviceProvider);
                configure(optionBuilder);
                return new SqlSugarScope(optionBuilder);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
