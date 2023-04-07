using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarSplitTableRepository<TAggregateRoot, TKey> : SqlSugarRepository<TAggregateRoot, TKey>, ISplitTableRepository<TAggregateRoot, TKey>, IQuerableRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    {
        public virtual async Task<TAggregateRoot> FindByIdAsync(TKey key)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).InSingleAsync(key);
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).Where(t => t.Id.Equals(key)).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).FirstAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).FirstAsync(predicate);
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).Where(predicate).SplitTable(tabs => tabs).FirstAsync();
            }
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
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().AnyAsync(entity => entity.Id.Equals(key));
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => entity.Id.Equals(key)).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().AnyAsync(predicate);
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<bool> AnyAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().AnyAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().AnyAsync(predicate);
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<int> CountAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().CountAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().SplitTable(tabs => tabs).CountAsync();
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().CountAsync(predicate);
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).CountAsync();
            }
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
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Updateable(entity).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable(entity).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable(entities.ToList()).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommandAsync();
            }
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommandAsync();
            }
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
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Deleteable<TAggregateRoot>().In(key).ExecuteCommandAsync();
            }
            else
            {
                var entity = await FindByIdAsync(key);
                return await RemoveAsync(entity);
            }
        }

        public virtual async Task<int> RemoveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Deleteable(entity).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Deleteable(entity).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Deleteable(entities.ToList()).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Deleteable(entities.ToList()).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Deleteable(predicate).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Deleteable(predicate).SplitTable().ExecuteCommandAsync();
            }
        }
    }
}