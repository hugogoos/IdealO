using Ideal.Core.Mqtt.Configurations.Options;
using Microsoft.Extensions.Configuration;

namespace Ideal.Core.Mqtt.Configurations
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public ConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// MQTT配置
        /// </summary>
        public IEnumerable<MqttOption> MqttOptions => _configuration.GetSection("MqttOptions").Get<IEnumerable<MqttOption>>();
    }
}
