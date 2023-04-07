namespace Ideal.Core.Mqtt.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MqttClientRepositoryProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IMqttClientRepository MqttClientRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mqttClientRepository"></param>
        public MqttClientRepositoryProvider(IMqttClientRepository mqttClientRepository)
        {
            MqttClientRepository = mqttClientRepository;
        }
    }
}
