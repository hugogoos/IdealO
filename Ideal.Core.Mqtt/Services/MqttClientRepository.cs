using Ideal.Core.Mqtt.Configurations;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Text.Json;

namespace Ideal.Core.Mqtt.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MqttClientRepository : IMqttClientRepository
    {
        private readonly ILogger<MqttClientRepository> _logger;
        private readonly IDictionary<string, IEnumerable<IManagedMqttClient>> _managedMqttClients = new Dictionary<string, IEnumerable<IManagedMqttClient>>();
        private readonly List<ManagedMqttClientOptions> _optionBuilders;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="optionBuilders"></param>
        /// <param name="configManager"></param>
        public MqttClientRepository(ILogger<MqttClientRepository> logger, List<ManagedMqttClientOptions> optionBuilders, IConfigurationCenter configManager)
        {
            _logger = logger;
            _optionBuilders = optionBuilders;
            var mqttOptions = configManager.MqttOptions;

            foreach (var mqttOption in mqttOptions)
            {
                var mqttClients = new List<IManagedMqttClient>();
                var clientCount = mqttOption.ClientCount;
                for (var i = 0; i < clientCount; i++)
                {
                    var mqttClient = new MqttFactory().CreateManagedMqttClient();
                    mqttClients.Add(mqttClient);
                }

                _managedMqttClients.Add($"{mqttOption.Server}:{mqttOption.Port}", mqttClients);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var managedMqttClient in _managedMqttClients)
            {
                var keys = managedMqttClient.Key.Split(':');
                var optionBuilders = _optionBuilders.Where(m => ((MqttClientTcpOptions)m.ClientOptions.ChannelOptions).Server == keys[0]
                                                                && ((MqttClientTcpOptions)m.ClientOptions.ChannelOptions).Port == int.Parse(keys[1])).ToList();
                var mqttClients = managedMqttClient.Value;
                var count = mqttClients.Count();
                for (var i = 0; i < count; i++)
                {
                    try
                    {
                        var mqttClient = mqttClients.ElementAt(i);
                        await mqttClient.StartAsync(optionBuilders[i]);
                        _logger.LogError($"ClientId：{mqttClient.Options.ClientOptions.ClientId}，连接成功！");
                    }
                    catch (Exception exp)
                    {
                        _logger.LogError($"MQTT连接服务器失败 Msg：{JsonSerializer.Serialize(exp)}");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                foreach (var managedMqttClient in _managedMqttClients)
                {
                    foreach (var mqttClient in managedMqttClient.Value)
                    {
                        await mqttClient.StopAsync();
                        _logger.LogError($"ClientId：{mqttClient.Options.ClientOptions.ClientId}，停止成功！");
                        Console.WriteLine($"ClientId：{mqttClient.Options.ClientOptions.ClientId}，停止成功！");
                    }
                }
            }

            foreach (var managedMqttClient in _managedMqttClients)
            {
                foreach (var mqttClient in managedMqttClient.Value)
                {
                    await mqttClient.StopAsync();
                    _logger.LogError($"ClientId：{mqttClient.Options.ClientOptions.ClientId}，停止成功！");
                    Console.WriteLine($"ClientId：{mqttClient.Options.ClientOptions.ClientId}，停止成功！");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IEnumerable<IManagedMqttClient>> GetManagedMqttClients()
        {
            return _managedMqttClients;
        }
    }
}
