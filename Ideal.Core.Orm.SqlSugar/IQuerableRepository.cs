using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 查询仓储接口，一般不建议领域中具体的仓储接口继承此接口，仅在必须使用IQueryable的返回进行进一步查询时使用
    /// </summary>
    /// <typeparam name="TAggregateRoot">充当聚合根的实体类型</typeparam>
    /// <typeparam name="TKey">实体的主键类型</typeparam>
    public interface IQuerableRepository<TAggregateRoot, in TKey> : IDisposable
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns>实体的IQueryable结果</returns>
        ISugarQueryable<TAggregateRoot> Query();

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="orderByKeySelector">分页字段</param>
        /// <param name="orderByType">分页方式</param>
        /// <returns></returns>
        ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType);

        /// <summary>
        /// 查询满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>实体的IQueryable结果</returns>
        ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 查询满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <param name="orderByKeySelector">分页字段</param>
        /// <param name="orderByType">分页方式</param>
        /// <returns>实体的IQueryable结果</returns>
        ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType);
    }
}
