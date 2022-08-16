using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract class OrgSqlSugarRepositoryWithDeleteFilter<IOrgAggregateRoot, TKey> :
        OrgSqlSugarRepositoryWithAudit<IOrgAggregateRoot, TKey>
        where IOrgAggregateRoot : class, IOrgAggregateRoot<TKey>, IAuditable, ISoftDelete, new()
    {
        protected OrgContext OrgContext { get; }

        protected OrgSqlSugarRepositoryWithDeleteFilter(ISqlSugarClient context, OrgContext orgContext) : base(context, orgContext)
        {
        }

        public override ISugarQueryable<IOrgAggregateRoot> Query()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted);
        }

        public override ISugarQueryable<IOrgAggregateRoot> Query(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public override ISugarQueryable<IOrgAggregateRoot> Query(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate);
        }

        public override ISugarQueryable<IOrgAggregateRoot> Query(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public override async Task<IOrgAggregateRoot> FindByIdAsync(TKey key)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).InSingleAsync(key);
        }
        public virtual async Task<IOrgAggregateRoot> FirstOrDefaultAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).FirstAsync();
        }

        public virtual async Task<IOrgAggregateRoot> FirstOrDefaultAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).FirstAsync(predicate);
        }

        public override async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public override async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).ToListAsync();
            }
        }

        public override async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public override async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate).ToListAsync();
            }
        }

        public override async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        public override async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        public override async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        public override async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
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
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).AnyAsync(entity => entity.Id.Equals(key));
        }

        public override async Task<bool> ExistsAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).AnyAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().Where(entity => !entity.IsDeleted).AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).CountAsync(predicate);
        }

        public override async Task<int> RemoveByIdAsync(TKey key)
        {
            if (null != OrgWhere)
            {
                return await Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .SetColumnsIF(!string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName), entity => entity.UpdatedBy == OrgContext.GetUserIdAndName)
                                    .Where(OrgWhere)
                                    .Where(entity => entity.Id.Equals(key))
                                    .ExecuteCommandAsync();
            }
            else
            {
                return await Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .Where(entity => entity.Id.Equals(key)).ExecuteCommandAsync();
            }
        }

        public override async Task<int> RemoveAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                AddRemoveUserInfo(entity);

                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> RemoveAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddRemoveUserInfo(entities);

                return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> RemoveAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            if (null != OrgWhere)
            {
                return await Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .SetColumnsIF(!string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName), entity => entity.UpdatedBy == OrgContext.GetUserIdAndName)
                                    .Where(OrgWhere)
                                    .Where(predicate)
                                    .ExecuteCommandAsync();
            }
            else
            {
                return await Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .SetColumnsIF(!string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName), entity => entity.UpdatedBy == OrgContext.GetUserIdAndName)
                                    .Where(predicate)
                                    .ExecuteCommandAsync();
            }
        }

        protected void AddRemoveUserInfo(IOrgAggregateRoot entity)
        {
            if (string.IsNullOrWhiteSpace(entity.CreatedBy) && !string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName))
            {
                entity.UpdatedBy = OrgContext?.GetUserIdAndName;
            }

            entity.IsDeleted = true;
            entity.UpdatedTime = DateTime.Now;
        }

        protected void AddRemoveUserInfo(IEnumerable<IOrgAggregateRoot> entities)
        {
            foreach (var entity in entities)
            {
                AddRemoveUserInfo(entity);
            }
        }
    }
}