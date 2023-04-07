using MQTTnet.Extensions.ManagedClient;

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
        IDictionary<string, IEnumerable<IManagedMqttClient>> GetManagedMqttClients();

        /// <summary>
        /// 获取指定名称的所有客户端
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        IEnumerable<IManagedMqttClient> GetManagedMqttClientsByServerPort(string server, int port);

        /// <summary>
        /// 随机获取指定名称的一个客户端
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        IManagedMqttClient GetManagedMqttClientRandomByServerPort(string server, int port);

        /// <summary>
        /// 取指定名称的第一个客户端（针对本系统注册的所有客户端）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        IManagedMqttClient GetManagedMqttClientFirstByServerPort(string server, int port);

        /// <summary>
        /// 订阅主题（针对本系统注册的所有客户端）
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task SubscribeAsync(string topic);

        /// <summary>
        /// 取消订阅主题（针对本系统注册的所有客户端）
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <returns></returns>
        Task UnsubscribeAsync(string topic);

        /// <summary>
        /// 发布主题（针对本系统注册的所有组客户端，每组里面随机指定一个客户端）
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        Task PublishAsync(string topic, string message);

        /// <summary>
        /// 发布主题（针对本系统注册的所有组客户端，每组里面随机指定一个客户端）
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <returns></returns>
        Task PublishAsync(string topic, byte[] message);

        /// <summary>
        /// 发布主题（针对本系统注册的所有组客户端，每组里面随机指定一个客户端）
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        Task PublishAsync(string topic, string message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false);

        /// <summary>
        /// 发布主题（针对本系统注册的所有组客户端，每组里面随机指定一个客户端）
        /// </summary>
        /// <param name="topic">主题名</param>
        /// <param name="message">发布内容</param>
        /// <param name="mqttQualityOfServiceLevel">QoS服务质量</param>
        /// <param name="retainFlag">是否保留</param>
        /// <returns></returns>
        Task PublishAsync(string topic, byte[] message, int mqttQualityOfServiceLevel = 1, bool retainFlag = false);
    }
}
