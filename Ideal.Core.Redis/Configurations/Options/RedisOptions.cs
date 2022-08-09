namespace Ideal.Core.Redis.Configurations.Options
{
    /// <summary>
    /// 主从库 配置
    /// </summary>
    public class RedisOptions
    {
        public Default Default { get; set; }
    }

    public class Default
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
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 默认库
        /// </summary>
        public int DefaultDatabase { get; set; }
    }

}
