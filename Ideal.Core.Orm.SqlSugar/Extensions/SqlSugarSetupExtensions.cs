using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.SqlSugar.Configurations;
using Ideal.Core.Orm.SqlSugar.Options;
using Ideal.Core.Orm.SqlSugar.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

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
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigurationCenter, ConfigurationCenter>();
            var config = services.BuildServiceProvider().GetService<IConfigurationCenter>();
            var sqlSugarOption = config?.SqlSugarOptions;
            if (sqlSugarOption is null)
            {
                throw new ArgumentException("请检查SqlSugarOptions配置是否添加");
            }

            if (sqlSugarOption.SingleDbOption is not null)
            {
                services.AddSqlSugarSingleDbSetup();
            }
            else if (sqlSugarOption.MultiDbOptions is not null)
            {
                services.AddSqlSugarMultiDbSetup();
            }
            else if (sqlSugarOption.MasterSlaveOption is not null)
            {
                services.AddSqlSugaMasterSlaverDbSetup();
            }
            else
            {
                throw new ArgumentException("请检查SqlSugarOptions配置是否正确");
            }

            return services;
        }

        /// <summary>
        /// SqlSugar启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarSingleDbSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigurationCenter, ConfigurationCenter>();
            var config = services.BuildServiceProvider().GetService<IConfigurationCenter>();
            var option = config?.SqlSugarOptions?.SingleDbOption;
            if (option is null)
            {
                throw new ArgumentException("请检查SqlSugarOptions.SingleDbOption配置是否添加");
            }

            services.AddSqlSugarSetupWithConfig(optionBuilder =>
            {
                var config = new ConnectionConfig
                {
                    ConnectionString = option.ConnectionString,
                    DbType = option.DbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true,
                        DefaultCacheDurationInSeconds = 5,
                        IsAutoRemoveDataCache = true
                    }
                };

                if (!string.IsNullOrWhiteSpace(option.ConfigId))
                {
                    config.ConfigId = option.ConfigId;
                }

                optionBuilder.Add(config);
            });

            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            return services;
        }

        /// <summary>
        /// SqlSugar启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarMultiDbSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigurationCenter, ConfigurationCenter>();
            var config = services.BuildServiceProvider().GetService<IConfigurationCenter>();
            var options = config?.SqlSugarOptions?.MultiDbOptions;
            if (options is null || !options.Any())
            {
                throw new ArgumentException("请检查SqlSugarOptions.MultiDbOptions配置是否添加");
            }

            var unique = options.DistinctBy(m => m.ConfigId);

            if (unique.Count() != options.Length)
            {
                throw new ArgumentException("请确保ConfigId唯一");
            }

            services.AddSqlSugarSetupWithConfig(optionBuilder =>
            {
                foreach (var item in options)
                {
                    optionBuilder.Add(new ConnectionConfig
                    {
                        ConfigId = item.ConfigId,
                        ConnectionString = item.ConnectionString,
                        DbType = item.DbType,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.Attribute,
                        MoreSettings = new ConnMoreSettings()
                        {
                            IsWithNoLockQuery = true,
                            DefaultCacheDurationInSeconds = 5,
                            IsAutoRemoveDataCache = true
                        }
                    });
                }
            });
            return services;
        }

        /// <summary>
        /// SqlSugar 主从启动项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugaMasterSlaverDbSetup(this IServiceCollection services)
        {
            services.AddTransient<IConfigurationCenter, ConfigurationCenter>();
            var config = services.BuildServiceProvider().GetService<IConfigurationCenter>();
            var option = config?.SqlSugarOptions?.MasterSlaveOption;
            if (option is null)
            {
                throw new ArgumentException("请检查SqlSugarOptions.MasterSlaveOption配置是否添加");
            }

            services.AddSqlSugarSetupWithConfig(optionBuilder =>
            {
                optionBuilder.Add(new ConnectionConfig
                {
                    ConnectionString = option.MasterConnectionString,
                    DbType = option.DbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true,
                        DefaultCacheDurationInSeconds = 5,
                        IsAutoRemoveDataCache = true
                    },
                    SlaveConnectionConfigs = option.SlaveConnectionStrings.Select(connectionString => new SlaveConnectionConfig
                    {
                        HitRate = 10,
                        ConnectionString = connectionString
                    }).ToList()
                });
            });
            return services;
        }

        /// <summary>
        /// 客户端启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">Mqtt配置项</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarSetupWithConfig(this IServiceCollection services, Action<ConnectionConfigOptions> configure)
        {
            services.AddSingleton<ISqlSugarClient>(serviceProvider =>
            {
                var optionBuilder = new ConnectionConfigOptions(serviceProvider);
                configure(optionBuilder);
                return new SqlSugarDbContext(optionBuilder)
                {
                    IsSingleDb = 1 == optionBuilder.Count,
                };
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}

