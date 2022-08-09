namespace Ideal.Core.Mqtt.Configurations.Options
{
    /// <summary>
    /// MQTT配置
    /// </summary>
    public class MQTTOptions
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 监听端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        public string ClientId { get; set; }
    }
}
