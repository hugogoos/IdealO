using Microsoft.Extensions.Hosting;
using MQTTnet.Client;

namespace Ideal.Core.Mqtt.Services
{
    public interface IMqttClientRepository : IHostedService
    {
        IMqttClient GetMqttClient();
    }
}
