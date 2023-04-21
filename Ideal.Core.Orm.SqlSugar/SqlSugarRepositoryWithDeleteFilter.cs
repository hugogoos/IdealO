using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class SqlSugarRepositoryWithDeleteFilter<TAggregateRoot, TKey> :
        SqlSugarRepositoryWithAudit<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, IAuditable, ISoftDelete, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected SqlSugarRepositoryWithDeleteFilter(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ISugarQueryable<TAggregateRoot> Query()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public override ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public override ISugarQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TAggregateRoot FindById(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).InSingle(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TAggregateRoot FirstOrDefault()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override TAggregateRoot FirstOrDefault(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).With(SqlWith.NoLock).First(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TAggregateRoot> FindAll()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Where(predicate).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="orderByType"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Exists(TKey key)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any(entity => entity.Id.Equals(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override bool Exists(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Any()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Count()
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Queryable<TAggregateRoot>().Where(entity => !entity.IsDeleted).Count(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override int RemoveById(TKey key)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(entity => entity.Id.Equals(key)).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override int Remove(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(entity => entity.IsDeleted == true).SetColumns(entity => entity.UpdatedTime == DateTime.Now).Where(predicate).ExecuteCommand();
        }
    }
}