namespace Ideal.Core.Mqtt.Services
{
    public class MqttClientRepositoryProvider
    {
        public readonly IMqttClientRepository MqttClientRepository;

        public MqttClientRepositoryProvider(IMqttClientRepository mqttClientRepository)
        {
            MqttClientRepository = mqttClientRepository;
        }
    }
}
