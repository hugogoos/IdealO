using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract class SqlSugarRepository<TAggregateRoot, TKey> : ISplitTableRepository<TAggregateRoot, TKey>, IQuerableRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    {
        protected ISqlSugarClient Context { get; }

        protected SqlSugarRepository(ISqlSugarClient context)
        {
            Context = context;
        }

        protected ISugarQueryable<TAggregateRoot> SugarQueryable => Context.Queryable<TAggregateRoot>();

        public virtual ISugarQueryable<TAggregateRoot> Query()
        {
            return Context.Queryable<TAggregateRoot>();
        }

        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(predicate);
        }

        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public virtual async Task<TAggregateRoot> FindByIdAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).InSingleAsync(key);
        }

        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync()
        {
            return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).FirstAsync();
        }

        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).FirstAsync(predicate);
        }

        public virtual async Task<IEnumerable<TAggregateRoot>> FindAllAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<TAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<TAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).ToListAsync();
            }
        }

        public virtual async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>();

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

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

        public virtual async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>();

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

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

        public virtual async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>().Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

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

        public virtual async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<TAggregateRoot>().Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

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

        public virtual async Task<bool> ExistsAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync(entity => entity.Id.Equals(key));
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync()
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await Context.Queryable<TAggregateRoot>().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().CountAsync(predicate);
        }

        public virtual async Task<int> CreateAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Insertable(entity).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Insertable(entity).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> CreateAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Insertable(entities.ToList()).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Insertable(entities.ToList()).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> SaveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Storageable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> SaveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Storageable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveByIdAsync(TKey key)
        {
            return await Context.Deleteable<TAggregateRoot>().In(key).ExecuteCommandAsync();
        }

        public virtual async Task<int> RemoveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return await Context.Deleteable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return await Context.Deleteable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Deleteable(predicate).ExecuteCommandAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}