using Ideal.Core.Orm.SqlSugar.Configurations.Options;
using Microsoft.Extensions.Configuration;

namespace Ideal.Core.Orm.SqlSugar.Configurations
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class ConfigurationCenter : IConfigurationCenter
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public ConfigurationCenter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 字符串链接
        /// </summary>
        public string ConnectionString => _configuration.GetSection("ConnectionString").Get<string>();

        /// <summary>
        /// 数据库主从连接字符串
        /// </summary>
        public MasterSlaveOption ConnectionStrings => _configuration.GetSection("ConnectionStringSettings").Get<MasterSlaveOption>();


        /// <summary>
        /// 数据库配置
        /// </summary>
        public SqlSugarOptions SqlSugarOptions => _configuration.GetSection("SqlSugarOptions").Get<SqlSugarOptions>();
    }
}
