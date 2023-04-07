using Microsoft.Extensions.Hosting;
using MQTTnet.Extensions.ManagedClient;

namespace Ideal.Core.Mqtt.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMqttClientRepository : IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, IEnumerable<IManagedMqttClient>> GetManagedMqttClients();
    }
}
