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
    public abstract partial class SqlSugarRepositoryWithDeleteFilter<TAggregateRoot, TKey> :
        SqlSugarRepositoryWithAudit<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, IAuditable, ISoftDelete, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override async Task<TAggregateRoot> FindByIdAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).InSingleAsync(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<TAggregateRoot> FirstOrDefaultAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).FirstAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<TAggregateRoot> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).FirstAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<TAggregateRoot>> FindAllAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<IEnumerable<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted);

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
        public override async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate);

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
        public override async Task<bool> ExistsAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync(entity => entity.Id.Equals(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> AnyAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<int> CountAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).CountAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).CountAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override async Task<int> RemoveByIdAsync(TKey key)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(entity => entity.Id.Equals(key)).ExecuteCommandAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task<int> RemoveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedTime = DateTime.Now;
                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public override async Task<int> RemoveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var es = entities.ToList();
                es.ForEach(entity =>
                {
                    entity.IsDeleted = true;
                    entity.UpdatedTime = DateTime.Now;
                });

                return await Context.Updateable(es).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<int> RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(predicate).ExecuteCommandAsync();
        }
    }
}