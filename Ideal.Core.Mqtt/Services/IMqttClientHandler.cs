using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;

namespace Ideal.Core.Mqtt.Services
{
    /// <summary>
    /// Mqtt客户端Handler
    /// </summary>
    public interface IMqttClientHandler : IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {
    }
}
