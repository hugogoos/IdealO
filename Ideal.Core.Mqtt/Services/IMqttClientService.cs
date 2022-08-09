using MQTTnet.Client;
using System.Threading.Tasks;

namespace Ideal.Core.Mqtt.Services
{
    /// <summary>
    /// MqttClientService
    /// </summary>
    public interface IMqttClientService
    {
        /// <summary>
        /// 获取IMqttClient对象
        /// </summary>
        /// <returns></returns>
        IMqttClient GetMqttClient();

        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task SubscribeAsync(string topic);

        /// <summary>
        /// 不带群组的共享订阅
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task SubscribeQueueAsync(string topic);

        /// <summary>
        /// 带群组的共享订阅
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task SubscribeShareAsync(string topic);

        /// <summary>
        /// 取消订阅主题
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task UnsubscribeAsync(string topic);

        /// <summary>
        /// 取消不带群组的共享订阅
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task UnsubscribeQueueAsync(string topic);

        /// <summary>
        /// 取消带群组的共享订阅
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task UnsubscribeShareAsync(string topic);

        /// <summary>
        /// 发布主题
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        Task PublishAsync(string topic, string message);

        /// <summary>
        /// 发布主题
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        Task PublishAsync(string topic, byte[] message);

        /// <summary>
        /// 发布主题
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        Task PublishAsync(string topic, string message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false);

        /// <summary>
        /// 发布主题
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        Task PublishAsync(string topic, byte[] message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false);
    }
}
