using CSRedis;

namespace Ideal.Core.Redis
{
    /// <summary>
    /// MqttClientService
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// CSRedis
        /// </summary>
        /// <returns></returns>
        CSRedisClient CSRedis { get; }

        #region 自定义方法

        /// <summary>
        /// 迭代哈希表中的键值对
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="action">委托</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        Task HScanAsync<T>(string key, Action<IEnumerable<(string, T)>> action, long cursor, string pattern = null, long? count = null);

        /// <summary>
        /// 获取迭代哈希表中的所有键值对
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        Task<IEnumerable<(string, T)>> HScanAsync<T>(string key, string pattern = null, long? count = null);
        #endregion


    }
}
