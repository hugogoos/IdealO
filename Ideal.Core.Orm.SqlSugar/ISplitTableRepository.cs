using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 异步仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">充当聚合根的实体类型</typeparam>
    /// <typeparam name="TKey">实体的主键类型</typeparam>
    public interface ISplitTableRepository<TAggregateRoot, in TKey> : IDisposable
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        /// <summary>
        /// 查找所有分表实体
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回所有分表实体的列表的异步任务</returns>
        Task<IEnumerable<TAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 查找满足条件的分表实体
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体列表结果的异步任务</returns>
        Task<IEnumerable<TAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 分页查找所有分表实体
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager);

        /// <summary>
        /// 分页查找满足条件的分表实体
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="predicate">条件谓词</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager);
    }
}