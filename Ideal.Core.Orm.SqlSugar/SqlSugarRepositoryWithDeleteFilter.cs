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
    public abstract class SqlSugarRepositoryWithDeleteFilter<TAggregateRoot, TKey> :
        SqlSugarRepositoryWithAudit<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, IAuditable, ISoftDelete, new()
    {
        protected SqlSugarRepositoryWithDeleteFilter(ISqlSugarClient context) : base(context)
        {
        }

        public override ISugarQueryable<TAggregateRoot> Query()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted);
        }

        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate);
        }

        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public override async Task<TAggregateRoot> FindByIdAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).InSingleAsync(key);
        }
        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).FirstAsync();
        }

        public virtual async Task<TAggregateRoot> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).FirstAsync(predicate);
        }

        public override async Task<IEnumerable<TAggregateRoot>> FindAllAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public override async Task<IEnumerable<TAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).ToListAsync();
            }
        }

        public override async Task<IEnumerable<TAggregateRoot>> FindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).SplitTable(tabs => tabs).ToListAsync();
            }
        }
        public override async Task<IEnumerable<TAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).ToListAsync();
            }
        }


        public override async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
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

        public override async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
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

        public override async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
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

        public override async Task<IPagedList<TAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
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

        public override async Task<bool> ExistsAsync(TKey key)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync(entity => entity.Id.Equals(key));
        }

        public override async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync(predicate);
        }


        public virtual async Task<bool> AnyAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).CountAsync(predicate);
        }

        public override async Task<int> RemoveByIdAsync(TKey key)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(entity => entity.Id.Equals(key)).ExecuteCommandAsync();
        }

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

        public override async Task<int> RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(predicate).ExecuteCommandAsync();
        }
    }
}