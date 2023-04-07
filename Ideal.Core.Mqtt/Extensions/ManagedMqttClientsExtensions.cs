using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System.Text;

namespace Ideal.Core.Mqtt.Extensions
{
    /// <summary>
    /// IEnumerable[IManagedMqttClient]扩展方法
    /// </summary>
    public static class ManagedMqttClientsExtensions
    {
        /// <summary>
        /// 订阅主题（针对指定的所有客户端）
        /// </summary>
        /// <param name="managedMqttClients"></param>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        public static async Task SubscribeAsync(this IEnumerable<IManagedMqttClient> managedMqttClients, string topic)
        {
            foreach (var managedMqttClient in managedMqttClients)
            {
                await managedMqttClient.SubscribeAsync(new MqttTopicFilter[] { new MqttTopicFilterBuilder().WithTopic(topic).Build() });
            }
        }

        /// <summary>
        /// 取消订阅主题（针对指定的所有客户端）
        /// </summary>
        /// <param name="managedMqttClients"></param>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        public static async Task UnsubscribeAsync(this IEnumerable<IManagedMqttClient> managedMqttClients, string topic)
        {
            foreach (var managedMqttClient in managedMqttClients)
            {
                await managedMqttClient.UnsubscribeAsync(topic);
            }
        }

        /// <summary>
        /// 发布主题（针对指定的所有客户端）
        /// </summary>
        /// <param name="managedMqttClients"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        public static Task PublishAsync(this IEnumerable<IManagedMqttClient> managedMqttClients, string topic, string message)
        {
            return PublishTopicAsync(managedMqttClients, topic, message, 1, false);
        }

        /// <summary>
        /// 发布主题（针对指定的所有客户端）
        /// </summary>
        /// <param name="managedMqttClients"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        public static Task PublishAsync(this IEnumerable<IManagedMqttClient> managedMqttClients, string topic, byte[] message)
        {
            return PublishTopicAsync(managedMqttClients, topic, message, 1, false);
        }

        /// <summary>
        /// 发布主题（针对指定的所有客户端）
        /// </summary>
        /// <param name="managedMqttClients"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        public static Task PublishAsync(this IEnumerable<IManagedMqttClient> managedMqttClients, string topic, string message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false)
        {
            return PublishTopicAsync(managedMqttClients, topic, message, mqttQualityOfServiceLevel, retainFlag);
        }

        /// <summary>
        /// 发布主题（针对指定的所有客户端）
        /// </summary>
        /// <param name="managedMqttClients"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        public static Task PublishAsync(this IEnumerable<IManagedMqttClient> managedMqttClients, string topic, byte[] message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false)
        {
            return PublishTopicAsync(managedMqttClients, topic, message, mqttQualityOfServiceLevel, retainFlag);
        }

        private static Task PublishTopicAsync(IEnumerable<IManagedMqttClient> managedMqttClients, string topic, string message, int mqttQualityOfServiceLevel, bool retainFlag)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            return PublishTopicAsync(managedMqttClients, topic, bytes, mqttQualityOfServiceLevel, retainFlag);
        }

        private static Task PublishTopicAsync(IEnumerable<IManagedMqttClient> managedMqttClients, string topic, byte[] message, int mqttQualityOfServiceLevel, bool retainFlag)
        {
            var messageObj = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(retainFlag)
                .Build();

            messageObj.QualityOfServiceLevel = (MqttQualityOfServiceLevel)mqttQualityOfServiceLevel;

            return GetManagedMqttClientRandom(managedMqttClients).EnqueueAsync(messageObj);
        }

        private static IManagedMqttClient GetManagedMqttClientRandom(IEnumerable<IManagedMqttClient> managedMqttClients)
        {
            var count = managedMqttClients.Count();
            if (count == 1)
            {
                return managedMqttClients.ElementAt(0);
            }

            return managedMqttClients.ElementAt(new Random().Next(count));
        }
    }
}
