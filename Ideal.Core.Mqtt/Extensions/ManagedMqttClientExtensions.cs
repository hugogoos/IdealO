using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System.Text;

namespace Ideal.Core.Mqtt.Extensions
{
    /// <summary>
    /// IManagedMqttClient扩展方法
    /// </summary>
    public static class ManagedMqttClientExtensions
    {
        /// <summary>
        /// 订阅主题（针对指定的客户端）
        /// </summary>
        /// <param name="managedMqttClient"></param>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        public static Task SubscribeAsync(this IManagedMqttClient managedMqttClient, string topic)
        {
            return managedMqttClient.SubscribeAsync(new MqttTopicFilter[] { new MqttTopicFilterBuilder().WithTopic(topic).Build() });
        }

        /// <summary>
        /// 取消订阅主题（针对指定的客户端）
        /// </summary>
        /// <param name="managedMqttClient"></param>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        public static Task UnsubscribeAsync(this IManagedMqttClient managedMqttClient, string topic)
        {
            return managedMqttClient.UnsubscribeAsync(topic);
        }

        /// <summary>
        /// 发布主题（针对指定的客户端）
        /// </summary>
        /// <param name="managedMqttClient"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        public static Task PublishAsync(this IManagedMqttClient managedMqttClient, string topic, string message)
        {
            return PublishTopicAsync(managedMqttClient, topic, message, 1, false);
        }

        /// <summary>
        /// 发布主题（针对指定的客户端）
        /// </summary>
        /// <param name="managedMqttClient"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        public static Task PublishAsync(this IManagedMqttClient managedMqttClient, string topic, byte[] message)
        {
            return PublishTopicAsync(managedMqttClient, topic, message, 1, false);
        }

        /// <summary>
        /// 发布主题（针对指定的客户端）
        /// </summary>
        /// <param name="managedMqttClient"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        public static Task PublishAsync(this IManagedMqttClient managedMqttClient, string topic, string message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false)
        {
            return PublishTopicAsync(managedMqttClient, topic, message, mqttQualityOfServiceLevel, retainFlag);
        }

        /// <summary>
        /// 发布主题（针对指定的客户端）
        /// </summary>
        /// <param name="managedMqttClient"></param>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        public static Task PublishAsync(this IManagedMqttClient managedMqttClient, string topic, byte[] message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false)
        {
            return PublishTopicAsync(managedMqttClient, topic, message, mqttQualityOfServiceLevel, retainFlag);
        }

        private static Task PublishTopicAsync(IManagedMqttClient managedMqttClient, string topic, string message, int mqttQualityOfServiceLevel, bool retainFlag)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            return PublishTopicAsync(managedMqttClient, topic, bytes, mqttQualityOfServiceLevel, retainFlag);
        }

        private static Task PublishTopicAsync(IManagedMqttClient managedMqttClient, string topic, byte[] message, int mqttQualityOfServiceLevel, bool retainFlag)
        {
            var messageObj = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(retainFlag)
                .Build();

            messageObj.QualityOfServiceLevel = (MqttQualityOfServiceLevel)mqttQualityOfServiceLevel;

            return managedMqttClient.EnqueueAsync(messageObj);
        }
    }
}
