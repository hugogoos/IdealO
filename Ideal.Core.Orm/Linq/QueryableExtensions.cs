using System.Linq.Expressions;

namespace Ideal.Core.Orm.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 当满足特定条件时执行查询
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">查询对象</param>
        /// <param name="condition">需要满足的条件</param>
        /// <param name="predicate">满足条件时执行的查询表达式</param>
        /// <returns>结果集</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
