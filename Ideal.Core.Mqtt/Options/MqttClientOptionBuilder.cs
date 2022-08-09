using MQTTnet.Client.Options;
using System;

namespace Ideal.Core.Mqtt.Options
{
    /// <summary>
    /// Mqtt配置项创建
    /// </summary>
    public class MqttClientOptionBuilder : MqttClientOptionsBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public MqttClientOptionBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
