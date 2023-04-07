using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarSplitTableRepository<TAggregateRoot, TKey> : SqlSugarRepository<TAggregateRoot, TKey>, ISplitTableRepository<TAggregateRoot, TKey>, IQuerableRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    {
        protected ISqlSugarClient Context { get; }

        protected SqlSugarSplitTableRepository(ISqlSugarClient context) : base(context)
        {
            Context = context;
        }

        protected ISugarQueryable<TAggregateRoot> SugarQueryable => Context.Queryable<TAggregateRoot>();

        public virtual TAggregateRoot FindById(TKey key)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).InSingle(key);
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).Where(t => t.Id.Equals(key)).SplitTable(tabs => tabs).First();
            }
        }

        public virtual TAggregateRoot FirstOrDefault()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).First();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).SplitTable(tabs => tabs).First();
            }
        }

        public virtual TAggregateRoot FirstOrDefault(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).First(predicate);
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().With(SqlWith.NoLock).Where(predicate).SplitTable(tabs => tabs).First();
            }
        }

        public virtual IEnumerable<TAggregateRoot> FindAll()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().ToList();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().SplitTable(tabs => tabs).ToList();
            }
        }

        public virtual IEnumerable<TAggregateRoot> FindAll(DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().SplitTable(startTime, endTime).ToList();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().ToList();
            }
        }

        public virtual IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).ToList();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).ToList();
            }
        }

        public virtual IEnumerable<TAggregateRoot> FindAll(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(startTime, endTime).ToList();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).ToList();
            }
        }

        public virtual IPagedList<TAggregateRoot> PagedFindAll(Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>();

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

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

        public virtual IPagedList<TAggregateRoot> PagedFindAll(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>();

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

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

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

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

        public virtual IPagedList<TAggregateRoot> PagedFindAll(DateTime startTime, DateTime endTime, Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = 0;
            var query = Context.Queryable<TAggregateRoot>().Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

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
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Any(entity => entity.Id.Equals(key));
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().Where(entity => entity.Id.Equals(key)).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual bool Exists(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Any(predicate);
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual bool Any()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Any();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().SplitTable(tabs => tabs).Any();
            }
        }

        public virtual bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Any(predicate);
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).Any();
            }
        }

        public virtual int Count()
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Count();
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().SplitTable(tabs => tabs).Count();
            }
        }

        public virtual int Count(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Queryable<TAggregateRoot>().Count(predicate);
            }
            else
            {
                return Context.Queryable<TAggregateRoot>().Where(predicate).SplitTable(tabs => tabs).Count();
            }
        }

        public virtual int Create(TAggregateRoot entity)
        {
            if (entity != null)
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
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

        public virtual int Create(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
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

        public virtual int Update(TAggregateRoot entity)
        {
            if (entity != null)
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
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

        public virtual int Update(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
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

        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
            }
            else
            {
                return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommand();
            }
        }

        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommand();
            }
            else
            {
                return Context.Updateable<TAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommand();
            }
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
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Deleteable<TAggregateRoot>().In(key).ExecuteCommand();
            }
            else
            {
                var entity = FindById(key);
                return Remove(entity);
            }
        }

        public virtual int Remove(TAggregateRoot entity)
        {
            if (entity != null)
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
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

        public virtual int Remove(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
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

        public virtual int Remove(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<TAggregateRoot>();
            if (!isSplitTable)
            {
                return Context.Deleteable(predicate).ExecuteCommand();
            }
            else
            {
                return Context.Deleteable(predicate).SplitTable().ExecuteCommand();
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}