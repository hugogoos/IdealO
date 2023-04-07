using Ideal.Core.Mqtt.Extensions;
using Microsoft.Extensions.Logging;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace Ideal.Core.Mqtt.Services
{
    using Ideal.Core.Mqtt.Extensions;

    /// <summary>
    /// 
    /// </summary>
    public class MqttClientService : IMqttClientService
    {
        private readonly ILogger<MqttClientService> logger;
        private readonly IDictionary<string, IEnumerable<IManagedMqttClient>> managedMqttClients;

        /// <summary>
        /// 
        /// </summary>
        public MqttClientService(ILogger<MqttClientService> logger, MqttClientRepositoryProvider mqttClientRepositoryProvider)
        {
            this.logger = logger;
            managedMqttClients = mqttClientRepositoryProvider.MqttClientRepository.GetManagedMqttClients();
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, IEnumerable<IManagedMqttClient>> GetManagedMqttClients()
        {
            return managedMqttClients;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public IEnumerable<IManagedMqttClient> GetManagedMqttClientsByServerPort(string server, int port)
        {
            var key = $"{server}:{port}";
            if (managedMqttClients.ContainsKey(key))
            {
                return managedMqttClients[key];
            }
            else
            {
                return new List<IManagedMqttClient>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public IManagedMqttClient GetManagedMqttClientRandomByServerPort(string server, int port)
        {
            var key = $"{server}:{port}";
            if (managedMqttClients.ContainsKey(key))
            {
                var mqttClients = managedMqttClients[key];
                var count = mqttClients.Count();
                if (count == 1)
                {
                    return mqttClients.ElementAt(0);
                }
                else
                {
                    return mqttClients.ElementAt(new Random().Next(count));
                }
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public IManagedMqttClient GetManagedMqttClientFirstByServerPort(string server, int port)
        {
            var key = $"{server}:{port}";
            if (managedMqttClients.ContainsKey(key))
            {
                return managedMqttClients[key].ElementAt(0);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Task PublishAsync(string topic, string message)
        {
            return PublishTopicAsync(topic, message, 1, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public Task PublishAsync(string topic, byte[] message)
        {
            return PublishTopicAsync(topic, message, 1, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public Task PublishAsync(string topic, string message, int mqttQualityOfServiceLevel = 1, bool RetainFlag = false)
        {
            return PublishTopicAsync(topic, message, mqttQualityOfServiceLevel, RetainFlag);
        }

        /// <summary>
        /// 
        /// </summary>
        public Task PublishAsync(string topic, byte[] message, int mqttQualityOfServiceLevel = 1, bool RetainFlag = false)
        {
            return PublishTopicAsync(topic, message, mqttQualityOfServiceLevel, RetainFlag);
        }

        /// <summary>
        /// 
        /// </summary>
        private Task PublishTopicAsync(string topic, string message, int mqttQualityOfServiceLevel, bool RetainFlag)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                return PublishTopicAsync(topic, bytes, mqttQualityOfServiceLevel, RetainFlag);
            }
            catch (Exception ex)
            {
                logger.LogError($"发布MQTT消息失败！{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Task PublishTopicAsync(string topic, byte[] message, int mqttQualityOfServiceLevel, bool RetainFlag)
        {
            try
            {
                var tasks = managedMqttClients.Select(managedMqttClient =>
                               managedMqttClient.Value.PublishAsync(topic, message, mqttQualityOfServiceLevel, RetainFlag));

                return Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                logger.LogError($"发布MQTT消息失败！{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task SubscribeAsync(string topic)
        {
            try
            {
                foreach (var managedMqttClient in managedMqttClients)
                {
                    foreach (var mqttClient in managedMqttClient.Value)
                    {
                        await mqttClient.SubscribeAsync(topic);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task UnsubscribeAsync(string topic)
        {
            try
            {
                foreach (var managedMqttClient in managedMqttClients)
                {
                    foreach (var mqttClient in managedMqttClient.Value)
                    {
                        await mqttClient.UnsubscribeAsync(topic);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"取消订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }
    }
}
