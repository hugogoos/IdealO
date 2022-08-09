using Ideal.Core.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 异步仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">充当聚合根的实体类型</typeparam>
    /// <typeparam name="TKey">实体的主键类型</typeparam>
    public interface IRepository<TAggregateRoot, in TKey> : IDisposable
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="key">实体的主键</param>
        /// <returns>返回实体结果的异步任务</returns>
        Task<TAggregateRoot> FindByIdAsync(TKey key);

        /// <summary>
        /// 查找第一个实体
        /// </summary>
        /// <returns>返回实体结果的异步任务</returns>
        Task<TAggregateRoot> FirstOrDefaultAsync();

        /// <summary>
        /// 根据条件查找第一个实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体结果的异步任务</returns>
        Task<TAggregateRoot> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 查找所有实体
        /// </summary>
        /// <returns>返回所有实体的列表的异步任务</returns>
        Task<IEnumerable<TAggregateRoot>> FindAllAsync();

        /// <summary>
        /// 查找满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体列表结果的异步任务</returns>
        Task<IEnumerable<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 分页查找所有实体
        /// </summary>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager);

        /// <summary>
        /// 分页查找满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表结果</returns>
        Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager);

        /// <summary>
        /// 判断是否存在指定主键的实体
        /// </summary>
        /// <param name="key">实体的主键</param>
        /// <returns>返回是否存在的异步任务</returns>
        Task<bool> ExistsAsync(TKey key);

        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 判断是否存在实体
        /// </summary>
        /// <returns>是否存在</returns>
        Task<bool> AnyAsync();

        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 计算实体个数
        /// </summary>
        /// <returns>条数</returns>
        Task<int> CountAsync();

        /// <summary>
        /// 计算满足条件的实体个数
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">新的实体</param>
        /// <returns>已创建的实体</returns>
        Task<int> CreateAsync(TAggregateRoot entity);

        /// <summary>
        /// 批量创建实体
        /// </summary>
        /// <param name="entities">新的实体集合</param>
        /// <returns>已创建的实体集合</returns>
        Task<int> CreateAsync(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">待更新的实体</param>
        Task<int> UpdateAsync(TAggregateRoot entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">待更新的实体集合</param>
        Task<int> UpdateAsync(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">待保存的实体</param>
        Task<int> SaveAsync(TAggregateRoot entity);

        /// <summary>
        /// 批量保存实体
        /// </summary>
        /// <param name="entities">待保存的实体集合</param>
        Task<int> SaveAsync(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="key">待删除实体的主键</param>
        Task<int> RemoveByIdAsync(TKey key);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">待删除的实体</param>
        Task<int> RemoveAsync(TAggregateRoot entity);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">待删除的实体集合</param>
        Task<int> RemoveAsync(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 删除满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        Task<int> RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate);
    }
}