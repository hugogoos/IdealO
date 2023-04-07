using MQTTnet.Extensions.ManagedClient;

namespace Ideal.Core.Mqtt.Options
{
    /// <summary>
    /// 
    /// </summary>
    public class ManagedMqttClientOptionBuilder : List<ManagedMqttClientOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ManagedMqttClientOptionBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
