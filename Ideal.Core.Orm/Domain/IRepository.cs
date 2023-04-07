using Ideal.Core.Common.Paging;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.Domain
{
    public partial interface IRepository<TAggregateRoot, in TKey> : IDisposable
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="key">实体的主键</param>
        /// <returns>返回实体结果的异步任务</returns>
        TAggregateRoot FindById(TKey key);

        /// <summary>
        /// 查找第一个实体
        /// </summary>
        /// <returns>返回实体结果的异步任务</returns>
        TAggregateRoot FirstOrDefault();

        /// <summary>
        /// 根据条件查找第一个实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体结果的异步任务</returns>
        TAggregateRoot FirstOrDefault(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 查找所有实体
        /// </summary>
        /// <returns>返回所有实体的列表的异步任务</returns>
        IEnumerable<TAggregateRoot> FindAll();

        /// <summary>
        /// 查找满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体列表结果的异步任务</returns>
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 分页查找所有实体
        /// </summary>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager);

        /// <summary>
        /// 分页查找满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表结果</returns>
        IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager);

        /// <summary>
        /// 判断是否存在指定主键的实体
        /// </summary>
        /// <param name="key">实体的主键</param>
        /// <returns>返回是否存在的异步任务</returns>
        bool Exists(TKey key);

        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        bool Exists(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 判断是否存在实体
        /// </summary>
        /// <returns>是否存在</returns>
        bool Any();

        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        bool Any(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 计算实体个数
        /// </summary>
        /// <returns>条数</returns>
        int Count();

        /// <summary>
        /// 计算满足条件的实体个数
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        int Count(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">新的实体</param>
        /// <returns>已创建的实体</returns>
        int Create(TAggregateRoot entity);

        /// <summary>
        /// 批量创建实体
        /// </summary>
        /// <param name="entities">新的实体集合</param>
        /// <returns>已创建的实体集合</returns>
        int Create(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">待更新的实体</param>
        int Update(TAggregateRoot entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">待更新的实体集合</param>
        int Update(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 批量更新实体列
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        int UpdateColumns(Expression<Func<TAggregateRoot, bool>> predicate);

        /// <summary>
        /// 批量更新实体列
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate);

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="entity">待保存的实体</param>
        int Save(TAggregateRoot entity);

        /// <summary>
        /// 批量保存实体
        /// </summary>
        /// <param name="entities">待保存的实体集合</param>
        int Save(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="key">待删除实体的主键</param>
        int RemoveById(TKey key);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">待删除的实体</param>
        int Remove(TAggregateRoot entity);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">待删除的实体集合</param>
        int Remove(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 删除满足条件的实体
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        int Remove(Expression<Func<TAggregateRoot, bool>> predicate);
    }
}