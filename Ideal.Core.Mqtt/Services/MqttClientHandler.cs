using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System.Threading.Tasks;

namespace Ideal.Core.Mqtt.Services
{
    public class MqttClientHandler : IMqttClientHandler
    {
        private readonly ILogger<MqttClientHandler> logger;
        private readonly IMqttClientService mqttClientService;
        private readonly IMqttClient mqttClient;

        public MqttClientHandler(ILogger<MqttClientHandler> logger, IMqttClientService mqttClientService)
        {
            this.logger = logger;
            this.mqttClientService = mqttClientService;
            mqttClient = mqttClientService.GetMqttClient();
            ConfigureMqttClient();
        }

        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }

        public virtual async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
        }

        public virtual async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
        }

        public virtual async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
        }
    }
}
