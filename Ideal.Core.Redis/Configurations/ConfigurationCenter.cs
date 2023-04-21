using Ideal.Core.Redis.Configurations.Options;
using Microsoft.Extensions.Configuration;

namespace Ideal.Core.Redis.Configurations
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
        public RedisOptions RedisOptions => _configuration.GetSection("RedisOptions").Get<RedisOptions>();
    }
}
