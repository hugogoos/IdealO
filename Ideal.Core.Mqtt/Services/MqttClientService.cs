using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ideal.Core.Mqtt.Services
{
    public class MqttClientService : IMqttClientService
    {
        private static readonly string QueueSubscriptionPrefix = "$queue/";
        private static readonly string ShareSubscriptionPrefix = "$share/";
        private readonly ILogger<MqttClientService> logger;
        private readonly IMqttClient mqttClient;

        public MqttClientService(ILogger<MqttClientService> logger, MqttClientRepositoryProvider mqttClientRepositoryProvider)
        {
            this.logger = logger;
            mqttClient = mqttClientRepositoryProvider.MqttClientRepository.GetMqttClient();
        }

        public IMqttClient GetMqttClient()
        {
            return mqttClient;
        }

        public async Task PublishAsync(string topic, string message)
        {
            await PublishTopicAsync(topic, message, 1, false);
        }

        public async Task PublishAsync(string topic, byte[] message)
        {
            await PublishTopicAsync(topic, message, 1, false);
        }

        public async Task PublishAsync(string topic, string message, int mqttQualityOfServiceLevel = 1, bool RetainFlag = false)
        {
            await PublishTopicAsync(topic, message, mqttQualityOfServiceLevel, RetainFlag);
        }

        public async Task PublishAsync(string topic, byte[] message, int mqttQualityOfServiceLevel = 1, bool RetainFlag = false)
        {
            await PublishTopicAsync(topic, message, mqttQualityOfServiceLevel, RetainFlag);
        }

        private async Task PublishTopicAsync(string topic, string message, int mqttQualityOfServiceLevel, bool RetainFlag)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await PublishTopicAsync(topic, bytes, mqttQualityOfServiceLevel, RetainFlag);
            }
            catch (Exception ex)
            {
                logger.LogError($"发布MQTT消息失败！{Environment.NewLine}{ex.Message}{Environment.NewLine}");
            }
        }

        private async Task PublishTopicAsync(string topic, byte[] message, int mqttQualityOfServiceLevel, bool RetainFlag)
        {
            try
            {
                var messageObj = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .WithRetainFlag(RetainFlag)
                    .Build();

                messageObj.QualityOfServiceLevel = (MqttQualityOfServiceLevel)mqttQualityOfServiceLevel;

                await mqttClient.PublishAsync(messageObj, CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError($"发布MQTT消息失败！{Environment.NewLine}{ex.Message}{Environment.NewLine}");
            }
        }

        public async Task SubscribeAsync(string topic)
        {
            try
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            }
            catch (Exception ex)
            {
                logger.LogError($"订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }

        public async Task SubscribeQueueAsync(string topic)
        {
            try
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic($"{QueueSubscriptionPrefix}{topic}").Build());
            }
            catch (Exception ex)
            {
                logger.LogError($"Queue共享订阅订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }

        public async Task SubscribeShareAsync(string topic)
        {
            try
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic($"{ShareSubscriptionPrefix}{topic}").Build());
            }
            catch (Exception ex)
            {
                logger.LogError($"Queue共享订阅订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }


        public async Task UnsubscribeAsync(string topic)
        {
            try
            {
                await mqttClient.UnsubscribeAsync(topic);
            }
            catch (Exception ex)
            {
                logger.LogError($"取消订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }

        public async Task UnsubscribeQueueAsync(string topic)
        {
            try
            {
                await mqttClient.UnsubscribeAsync($"{QueueSubscriptionPrefix}{topic}");
            }
            catch (Exception ex)
            {
                logger.LogError($"取消不带群组的共享订阅，订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }

        public async Task UnsubscribeShareAsync(string topic)
        {
            try
            {
                await mqttClient.UnsubscribeAsync($"{ShareSubscriptionPrefix}{topic}");
            }
            catch (Exception ex)
            {
                logger.LogError($"取消不带群组的共享订阅，订阅MQTT主题({topic})失败！{ex.Message}");
            }
        }
    }
}
