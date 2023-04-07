using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract partial class OrgSqlSugarSplitTableRepository<IOrgAggregateRoot, TKey> : OrgSqlSugarRepository<IOrgAggregateRoot, TKey>, ISplitTableRepository<IOrgAggregateRoot, TKey>, IQuerableRepository<IOrgAggregateRoot, TKey>, IRepository<IOrgAggregateRoot, TKey>
        where IOrgAggregateRoot : class, IOrgAggregateRoot<TKey>, new()
    {
        protected ISqlSugarClient Context { get; }

        protected OrgContext OrgContext { get; }

        protected readonly Expression<Func<IOrgAggregateRoot, bool>> OrgWhere;

        protected OrgSqlSugarSplitTableRepository(ISqlSugarClient context, OrgContext orgContext) : base(context, orgContext)
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
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).InSingle(key);
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).Where(t => t.Id.Equals(key)).SplitTable(tabs => tabs).First();
            }
        }

        public virtual IOrgAggregateRoot FirstOrDefault()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).First();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).SplitTable(tabs => tabs).First();
            }
        }

        public virtual IOrgAggregateRoot FirstOrDefault(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).First(predicate);
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).With(SqlWith.NoLock).SplitTable(tabs => tabs).First();
            }
        }

        public virtual IEnumerable<IOrgAggregateRoot> FindAll()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).ToList();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(tabs => tabs).ToList();
            }
        }

        public virtual IEnumerable<IOrgAggregateRoot> FindAll(DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(startTime, endTime).ToList();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).ToList();
            }
        }

        public virtual IEnumerable<IOrgAggregateRoot> FindAll(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).ToList();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).ToList();
            }
        }

        public virtual IEnumerable<IOrgAggregateRoot> FindAll(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(startTime, endTime).ToList();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).ToList();
            }
        }

        public virtual IPagedList<IOrgAggregateRoot> PagedFindAll(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

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

        public virtual IPagedList<IOrgAggregateRoot> PagedFindAll(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

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

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

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

        public virtual IPagedList<IOrgAggregateRoot> PagedFindAll(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

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
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any(entity => entity.Id.Equals(key));
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => entity.Id.Equals(key)).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual bool Exists(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any(predicate);
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual bool Any()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual bool Any(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Any(predicate);
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual int Count()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Count();
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(tabs => tabs).Count();
            }
        }

        public virtual int Count(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Count(predicate);
            }
            else
            {
                return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).Count();
            }
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

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return Context.Insertable(entity).ExecuteCommand();
                }
                else
                {
                    return Context.Insertable(entity).SplitTable().ExecuteCommand();
                }
            }

            return 0;
        }

        public virtual int Create(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return Context.Insertable(entities.ToList()).ExecuteCommand();
                }
                else
                {
                    return Context.Insertable(entities.ToList()).SplitTable().ExecuteCommand();
                }
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

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return Context.Updateable(entity).ExecuteCommand();
                }
                else
                {
                    return Context.Updateable(entity).SplitTable().ExecuteCommand();
                }
            }

            return 0;
        }

        public virtual int Update(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return Context.Updateable(entities.ToList()).ExecuteCommand();
                }
                else
                {
                    return Context.Updateable(entities.ToList()).SplitTable().ExecuteCommand();
                }
            }

            return 0;
        }

        public virtual int UpdateColumns(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
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
            else
            {
                if (null != OrgWhere)
                {
                    return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).SplitTable().ExecuteCommand();
                }
                else
                {
                    return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommand();
                }
            }
        }

        public virtual int UpdateColumns(Expression<Func<IOrgAggregateRoot, IOrgAggregateRoot>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
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
            else
            {
                if (null != OrgWhere)
                {
                    return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).SplitTable().ExecuteCommand();
                }
                else
                {
                    return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommand();
                }
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
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
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
            else
            {
                var entity = FindById(key);
                return Remove(entity);
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

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return Context.Deleteable(entity).ExecuteCommand();
                }
                else
                {
                    return Context.Deleteable(entity).SplitTable().ExecuteCommand();
                }
            }

            return 0;
        }

        public virtual int Remove(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return Context.Deleteable(entities.ToList()).ExecuteCommand();
                }
                else
                {
                    return Context.Deleteable(entities.ToList()).SplitTable().ExecuteCommand();
                }
            }

            return 0;
        }

        public virtual int Remove(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
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
            else
            {
                if (null != OrgWhere)
                {
                    return Context.Deleteable(predicate).Where(OrgWhere).SplitTable().ExecuteCommand();
                }
                else
                {
                    return Context.Deleteable(predicate).SplitTable().ExecuteCommand();
                }
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