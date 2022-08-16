using SqlSugar;
using System;
using System.Collections.Concurrent;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 类 帮助
    /// </summary>
    public class ClassHelper
    {
        private static readonly ConcurrentDictionary<Type, bool> _splitTableConcurrentDictionary = new();

        /// <summary>
        /// 是否是分表实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsSplitTable<T>()
        {
            var type = typeof(T);
            return _splitTableConcurrentDictionary.GetOrAdd(type, (key) =>
            {
                var result = type.IsDefined(typeof(SplitTableAttribute), true);
                return result;
            });
        }

    }
}
