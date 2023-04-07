using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract partial class OrgSqlSugarRepositoryWithDeleteFilter<IOrgAggregateRoot, TKey> :
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

        public override IOrgAggregateRoot FindById(TKey key)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).InSingle(key);
        }

        public virtual IOrgAggregateRoot FirstOrDefault()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).First();
        }

        public virtual IOrgAggregateRoot FirstOrDefault(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).First(predicate);
        }

        public override IEnumerable<IOrgAggregateRoot> FindAll()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).ToList();
        }

        public override IEnumerable<IOrgAggregateRoot> FindAll(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate).ToList();
        }

        public override IPagedList<IOrgAggregateRoot> PagedFindAll(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted);

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

        public override IPagedList<IOrgAggregateRoot> PagedFindAll(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Where(predicate);

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

        public override bool Exists(TKey key)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Any(entity => entity.Id.Equals(key));
        }

        public override bool Exists(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Any(predicate);
        }

        public virtual bool Any()
        {
            return Context.Queryable<IOrgAggregateRoot>().Where(entity => !entity.IsDeleted).Any();
        }

        public virtual bool Any(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Any(predicate);
        }

        public virtual int Count()
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Count();
        }

        public virtual int Count(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => !entity.IsDeleted).Count(predicate);
        }

        public override int RemoveById(TKey key)
        {
            if (null != OrgWhere)
            {
                return Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .SetColumnsIF(!string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName), entity => entity.UpdatedBy == OrgContext.GetUserIdAndName)
                                    .Where(OrgWhere)
                                    .Where(entity => entity.Id.Equals(key))
                                    .ExecuteCommand();
            }
            else
            {
                return Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .Where(entity => entity.Id.Equals(key)).ExecuteCommand();
            }
        }

        public override int Remove(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                AddRemoveUserInfo(entity);

                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        public override int Remove(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddRemoveUserInfo(entities);

                return Context.Updateable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public override int Remove(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            if (null != OrgWhere)
            {
                return Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .SetColumnsIF(!string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName), entity => entity.UpdatedBy == OrgContext.GetUserIdAndName)
                                    .Where(OrgWhere)
                                    .Where(predicate)
                                    .ExecuteCommand();
            }
            else
            {
                return Context.Updateable<IOrgAggregateRoot>()
                                    .SetColumns(entity => entity.IsDeleted == true)
                                    .SetColumns(entity => entity.UpdatedTime == DateTime.Now)
                                    .SetColumnsIF(!string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName), entity => entity.UpdatedBy == OrgContext.GetUserIdAndName)
                                    .Where(predicate)
                                    .ExecuteCommand();
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