using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarRepositoryWithDeleteFilter<TAggregateRoot, TKey> :
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

        public override TAggregateRoot FindById(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).InSingle(key);
        }
        public virtual TAggregateRoot FirstOrDefault()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).First();
        }

        public virtual TAggregateRoot FirstOrDefault(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).First(predicate);
        }

        public override IEnumerable<TAggregateRoot> FindAll()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).ToList();
        }

        public override IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).ToList();
        }

        public override IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted);

            var page = query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageList(pager.PageIndex, pager.PageSize, ref totalCount);
            var result = new PagedList<TAggregateRoot>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount,
                Entities = page
            };
            return result;
        }

        public override IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate);

            var page = query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageList(pager.PageIndex, pager.PageSize, ref totalCount);
            var result = new PagedList<TAggregateRoot>()
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
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any(entity => entity.Id.Equals(key));
        }

        public override bool Exists(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any(predicate);
        }


        public virtual bool Any()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any();
        }

        public virtual bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any(predicate);
        }

        public virtual int Count()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Count();
        }

        public virtual int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Count(predicate);
        }

        public override int RemoveById(TKey key)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(entity => entity.Id.Equals(key)).ExecuteCommand();
        }

        public override int Remove(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.UpdatedTime = DateTime.Now;
                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        public override int Remove(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var es = entities.ToList();
                es.ForEach(entity =>
                {
                    entity.IsDeleted = true;
                    entity.UpdatedTime = DateTime.Now;
                });

                return Context.Updateable(es).ExecuteCommand();
            }

            return 0;
        }

        public override int Remove(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(predicate).ExecuteCommand();
        }
    }
}