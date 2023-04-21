using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract partial class SqlSugarRepository<TAggregateRoot, TKey> : SqlSugarSqlRepository, IQuerableRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<TAggregateRoot> FindByIdAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).InSingleAsync(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync()
        {
            return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).FirstAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).FirstAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TAggregateRoot>> FindAllAsync()
        {
            return await Context.Queryable<TAggregateRoot>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public virtual async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>();

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<TAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public virtual async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>().Where(predicate);

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<TAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync(entity => entity.Id.Equals(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync()
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> CountAsync()
        {
            return await Context.Queryable<TAggregateRoot>().CountAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().CountAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Insertable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Insertable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommandAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommandAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnPredicate"></param>
        /// <param name="wherePredicate"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, bool>> columnPredicate, Expression<Func<TAggregateRoot, bool>> wherePredicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(columnPredicate).Where(wherePredicate).ExecuteCommandAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnPredicate"></param>
        /// <param name="wherePredicate"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, TAggregateRoot>> columnPredicate, Expression<Func<TAggregateRoot, bool>> wherePredicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(columnPredicate).Where(wherePredicate).ExecuteCommandAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> SaveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Storageable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> SaveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Storageable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveByIdAsync(TKey key)
        {
            return await Context.Deleteable<TAggregateRoot>().In(key).ExecuteCommandAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Deleteable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Deleteable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Deleteable(predicate).ExecuteCommandAsync();
        }
    }
}