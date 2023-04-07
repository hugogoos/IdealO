using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarRepository<TAggregateRoot, TKey> : IQuerableRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
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

        public virtual TAggregateRoot FindById(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).InSingle(key);
        }

        public virtual TAggregateRoot FirstOrDefault()
        {
            return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).First();
        }

        public virtual TAggregateRoot FirstOrDefault(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).First(predicate);
        }

        public virtual IEnumerable<TAggregateRoot> FindAll()
        {
            return Context.Queryable<TAggregateRoot>().ToList();
        }

        public virtual IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(predicate).ToList();
        }

        public virtual IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>();

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

        public virtual IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>().Where(predicate);

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

        public virtual bool Exists(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().Any(entity => entity.Id.Equals(key));
        }

        public virtual bool Exists(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Any(predicate);
        }

        public virtual bool Any()
        {
            return Context.Queryable<TAggregateRoot>().Any();
        }

        public virtual bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Any(predicate);
        }

        public virtual int Count()
        {
            return Context.Queryable<TAggregateRoot>().Count();
        }

        public virtual int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Count(predicate);
        }

        public virtual int Create(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Insertable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Create(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Insertable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Update(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Update(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Updateable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
        }

        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
        }

        public virtual int Save(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Storageable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Save(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Storageable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int RemoveById(TKey key)
        {
            return Context.Deleteable<TAggregateRoot>().In(key).ExecuteCommand();
        }

        public virtual int Remove(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Deleteable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Remove(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Deleteable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Remove(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Deleteable(predicate).ExecuteCommand();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}