using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarRepository<TAggregateRoot, TKey> : SqlSugarSqlRepository, IQuerableRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected SqlSugarScopeProvider Context { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        protected SqlSugarRepository(IDbContext dbContext) : base(dbContext)
        {
            var context = (SqlSugarDbContext)dbContext;
            var scopedContext = ((SqlSugarScope)context.ISqlSugarClient).ScopedContext;
            if (1 == context.ConnectionConfigs.Count)
            {
                Context = new SqlSugarScopeProvider(scopedContext.Context);
            }
            else
            {
                Context = scopedContext.GetConnectionScopeWithAttr<TAggregateRoot>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ISugarQueryable<TAggregateRoot> SugarQueryable => Context.Queryable<TAggregateRoot>();

        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<TAggregateRoot> SqlQuery(string sql)
        {
            return SqlQuery<TAggregateRoot>(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ISugarQueryable<TAggregateRoot> Query()
        {
            return Context.Queryable<TAggregateRoot>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TAggregateRoot FindById(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).InSingle(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual TAggregateRoot FirstOrDefault()
        {
            return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TAggregateRoot FirstOrDefault(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).First(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TAggregateRoot> FindAll()
        {
            return Context.Queryable<TAggregateRoot>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(predicate).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Exists(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().Any(entity => entity.Id.Equals(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Exists(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Any()
        {
            return Context.Queryable<TAggregateRoot>().Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return Context.Queryable<TAggregateRoot>().Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Count(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Create(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Insertable(entity).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Create(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Insertable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Update(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Updateable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnPredicate"></param>
        /// <param name="wherePredicate"></param>
        /// <returns></returns>
        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, bool>> columnPredicate, Expression<Func<TAggregateRoot, bool>> wherePredicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(columnPredicate).Where(wherePredicate).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnPredicate"></param>
        /// <param name="wherePredicate"></param>
        /// <returns></returns>
        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> columnPredicate, Expression<Func<TAggregateRoot, bool>> wherePredicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(columnPredicate).Where(wherePredicate).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Save(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Storageable(entity).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Save(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Storageable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int RemoveById(TKey key)
        {
            return Context.Deleteable<TAggregateRoot>().In(key).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Remove(TAggregateRoot entity)
        {
            if (entity != null)
            {
                return Context.Deleteable(entity).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Remove(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                return Context.Deleteable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Remove(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Deleteable(predicate).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}