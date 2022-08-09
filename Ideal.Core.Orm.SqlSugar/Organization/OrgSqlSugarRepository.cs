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
    public abstract class OrgSqlSugarRepository<IOrgAggregateRoot, TKey> : IQuerableRepository<IOrgAggregateRoot, TKey>, IRepository<IOrgAggregateRoot, TKey>
        where IOrgAggregateRoot : class, IOrgAggregateRoot<TKey>, new()
    {
        protected ISqlSugarClient Context { get; }

        protected OrgContext OrgContext { get; }

        protected readonly Expression<Func<IOrgAggregateRoot, bool>> OrgWhere;

        protected OrgSqlSugarRepository(ISqlSugarClient context, OrgContext orgContext)
        {
            Context = context;
            OrgContext = orgContext;

            if (orgContext.HasOrganization())
            {
                OrgWhere = o => orgContext.OrganizationIds.Contains(o.OrganizationId);
            }
        }

        public virtual ISugarQueryable<IOrgAggregateRoot> Query()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere);
        }

        public virtual ISugarQueryable<IOrgAggregateRoot> Query(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public virtual ISugarQueryable<IOrgAggregateRoot> Query(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate);
        }

        public virtual ISugarQueryable<IOrgAggregateRoot> Query(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        public virtual async Task<IOrgAggregateRoot> FindByIdAsync(TKey key)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).InSingleAsync(key);
        }

        public virtual async Task<IOrgAggregateRoot> FirstOrDefaultAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).FirstAsync();
        }

        public virtual async Task<IOrgAggregateRoot> FirstOrDefaultAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).FirstAsync(predicate);
        }

        public virtual async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).ToListAsync();
        }

        public virtual async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).ToListAsync();
        }

        public virtual async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var page = await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        public virtual async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var page = await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
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
            return await Context.Queryable<IOrgAggregateRoot>().AnyAsync(entity => entity.Id.Equals(key));
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).AnyAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).CountAsync(predicate);
        }

        public virtual async Task<int> CreateAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                return await Context.Insertable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> CreateAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return await Context.Insertable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> SaveAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                return await Context.Storageable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> SaveAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return await Context.Storageable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveByIdAsync(TKey key)
        {
            if (null != OrgWhere)
            {
                return await Context.Deleteable<IOrgAggregateRoot>().Where(OrgWhere).In(key).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Deleteable<IOrgAggregateRoot>().In(key).ExecuteCommandAsync();
            }
        }

        public virtual async Task<int> RemoveAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                return await Context.Deleteable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return await Context.Deleteable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            if (null != OrgWhere)
            {
                return await Context.Deleteable(predicate).Where(OrgWhere).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Deleteable(predicate).ExecuteCommandAsync();
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        protected bool IsIllegalOrg(IOrgAggregateRoot entity)
        {
            if (!OrgContext.HasOrganization())
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(entity.OrganizationId))
            {
                entity.OrganizationId = OrgContext.CurrentOrganizationId;
                return false;
            }

            if (!OrgContext.OrganizationIds.Contains(entity.OrganizationId))
            {
                return true;
            }

            return false;
        }

        protected void RemoveIllegalOrgs(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (OrgContext.HasOrganization())
            {
                var orgs = entities.ToList();
                foreach (var org in orgs)
                {
                    if (string.IsNullOrWhiteSpace(org.OrganizationId))
                    {
                        org.OrganizationId = OrgContext.CurrentOrganizationId;
                    }
                }

                orgs.RemoveAll(e => !OrgContext.OrganizationIds.Contains(e.OrganizationId));
            }
        }
    }
}