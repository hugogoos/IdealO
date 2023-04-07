using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract partial class OrgSqlSugarRepository<IOrgAggregateRoot, TKey> : IQuerableRepository<IOrgAggregateRoot, TKey>, IRepository<IOrgAggregateRoot, TKey>
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
                OrgWhere = o => orgContext.OrgIds.Contains(o.OrgId);
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

        public virtual IOrgAggregateRoot FindById(TKey key)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).InSingle(key);
        }

        public virtual IOrgAggregateRoot FirstOrDefault()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).First();
        }

        public virtual IOrgAggregateRoot FirstOrDefault(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).First(predicate);
        }

        public virtual IEnumerable<IOrgAggregateRoot> FindAll()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).ToList();
        }

        public virtual IEnumerable<IOrgAggregateRoot> FindAll(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).ToList();
        }

        public virtual IPagedList<IOrgAggregateRoot> PagedFindAll(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere);

            var page = query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageList(pager.PageIndex, pager.PageSize, ref totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount,
                Entities = page
            };
            return result;
        }

        public virtual IPagedList<IOrgAggregateRoot> PagedFindAll(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate);

            var page = query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageList(pager.PageIndex, pager.PageSize, ref totalCount);
            var result = new PagedList<IOrgAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount,
                Entities = page
            };
            return result;
        }

        public virtual bool Exists(TKey key)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any(entity => entity.Id.Equals(key));
        }

        public virtual bool Exists(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any(predicate);
        }

        public virtual bool Any()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any();
        }

        public virtual bool Any(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any(predicate);
        }

        public virtual int Count()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Count();
        }

        public virtual int Count(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Count(predicate);
        }

        public virtual int Create(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                return Context.Insertable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Create(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return Context.Insertable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Update(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Update(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return Context.Updateable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int UpdateColumns(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            if (null != OrgWhere)
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommand();
            }
            else
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
            }
        }

        public virtual int UpdateColumns(Expression<Func<IOrgAggregateRoot, IOrgAggregateRoot>> predicate)
        {
            if (null != OrgWhere)
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommand();
            }
            else
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
            }
        }

        public virtual int Save(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                return Context.Storageable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Save(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return Context.Storageable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int RemoveById(TKey key)
        {
            if (null != OrgWhere)
            {
                return Context.Deleteable<IOrgAggregateRoot>().Where(OrgWhere).In(key).ExecuteCommand();
            }
            else
            {
                return Context.Deleteable<IOrgAggregateRoot>().In(key).ExecuteCommand();
            }
        }

        public virtual int Remove(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                return Context.Deleteable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Remove(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                return Context.Deleteable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Remove(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            if (null != OrgWhere)
            {
                return Context.Deleteable(predicate).Where(OrgWhere).ExecuteCommand();
            }
            else
            {
                return Context.Deleteable(predicate).ExecuteCommand();
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

            if (string.IsNullOrWhiteSpace(entity.OrgId))
            {
                entity.OrgId = OrgContext.CurrentOrgId;
                return false;
            }

            if (!OrgContext.OrgIds.Contains(entity.OrgId))
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
                    if (string.IsNullOrWhiteSpace(org.OrgId))
                    {
                        org.OrgId = OrgContext.CurrentOrgId;
                    }
                }

                orgs.RemoveAll(e => !OrgContext.OrgIds.Contains(e.OrgId));
            }
        }
    }
}