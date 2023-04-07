using System.Collections.Concurrent;

namespace Ideal.Core.Common
{
    /// <summary>
    /// 内存基本阻塞队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueBlocking<T> where T : class
    {
        private static readonly BlockingCollection<T> Data = new();

        /// <summary>
        /// 是否完成
        /// </summary>
        /// <returns></returns>
        public static bool IsCompleted()
        {
            return Data != null && Data.IsCompleted;
        }

        /// <summary>
        /// 有元素
        /// </summary>
        /// <returns></returns>
        public static bool HasElement()
        {
            return Data != null && Data.Count > 0;
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Add(T element)
        {
            Data.Add(element);
            return true;
        }

        /// <summary>
        /// 取出元素
        /// </summary>
        /// <returns></returns>
        public static T Take()
        {
            return Data.Take();
        }
    }
}
