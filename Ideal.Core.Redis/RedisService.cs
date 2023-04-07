using CSRedis;
using Microsoft.Extensions.Logging;

namespace Ideal.Core.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisService : IRedisService
    {
        private readonly ILogger<CSRedisClient> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="redisClient"></param>
        public RedisService(ILogger<CSRedisClient> logger, CSRedisClient redisClient)
        {
            this.logger = logger;
            CSRedis = redisClient;
        }

        /// <summary>
        /// CSRedis
        /// </summary>
        /// <returns></returns>
        public CSRedisClient CSRedis { get; }

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
        public async Task HScanAsync<T>(string key, Action<IEnumerable<(string, T)>> action, long cursor, string pattern = null, long? count = null)
        {
            do
            {
                var hscan = await CSRedis.HScanAsync<T>(key, cursor, pattern, count);
                var items = hscan.Items.ToList();
                action(items);
                cursor = hscan.Cursor;
            } while (cursor != 0);

        }

        /// <summary>
        /// 获取迭代哈希表中的所有键值对
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<IEnumerable<(string, T)>> HScanAsync<T>(string key, string pattern = null, long? count = null)
        {
            var cursor = 0L;
            var result = new List<(string, T)>();
            do
            {
                var hscan = await CSRedis.HScanAsync<T>(key, cursor, pattern, count);
                var items = hscan.Items.ToList();
                result.AddRange(items);
                cursor = hscan.Cursor;
            } while (cursor != 0);

            return result;
        }
        #endregion

    }
}
