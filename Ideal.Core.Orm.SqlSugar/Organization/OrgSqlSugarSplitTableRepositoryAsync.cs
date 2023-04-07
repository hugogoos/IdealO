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
        public virtual async Task<IOrgAggregateRoot> FindByIdAsync(TKey key)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).InSingleAsync(key);
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).Where(t => t.Id.Equals(key)).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        public virtual async Task<IOrgAggregateRoot> FirstOrDefaultAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).FirstAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        public virtual async Task<IOrgAggregateRoot> FirstOrDefaultAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).With(SqlWith.NoLock).FirstAsync(predicate);
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).With(SqlWith.NoLock).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        public virtual async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<IOrgAggregateRoot>> FindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).ToListAsync();
            }
        }

        public virtual async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere);

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

        public virtual async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere);

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

        public virtual async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate);

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

        public virtual async Task<IPagedList<IOrgAggregateRoot>> PagedFindAllAsync(DateTime startTime, DateTime endTime, Expression<Func<IOrgAggregateRoot, bool>> predicate, Expression<Func<IOrgAggregateRoot, object>> orderByKeySelector, OrderByMode orderByType, Pager pager)
        {
            var totalCount = new RefAsync<int>();
            var query = Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate);

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

        public virtual async Task<bool> ExistsAsync(TKey key)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).AnyAsync(entity => entity.Id.Equals(key));
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(entity => entity.Id.Equals(key)).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).AnyAsync(predicate);
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<bool> AnyAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).AnyAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).AnyAsync(predicate);
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        public virtual async Task<int> CountAsync()
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).CountAsync();
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).SplitTable(tabs => tabs).CountAsync();
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).CountAsync(predicate);
            }
            else
            {
                return await Context.Queryable<IOrgAggregateRoot>().WhereIF(null != OrgWhere, OrgWhere).Where(predicate).SplitTable(tabs => tabs).CountAsync();
            }
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

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Insertable(entity).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Insertable(entity).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> CreateAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Insertable(entities.ToList()).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Insertable(entities.ToList()).SplitTable().ExecuteCommandAsync();
                }
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

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Updateable(entity).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable(entity).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable(entities.ToList()).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                if (null != OrgWhere)
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommandAsync();
                }
            }
            else
            {
                if (null != OrgWhere)
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).SplitTable().ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommandAsync();
                }
            }
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<IOrgAggregateRoot, IOrgAggregateRoot>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
            {
                if (null != OrgWhere)
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).ExecuteCommandAsync();
                }
            }
            else
            {
                if (null != OrgWhere)
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).Where(OrgWhere).SplitTable().ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).Where(t => t.Id != null).SplitTable().ExecuteCommandAsync();
                }
            }
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
            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
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
            else
            {
                var entity = await FindByIdAsync(key);
                return await RemoveAsync(entity);
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

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Deleteable(entity).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Deleteable(entity).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
                if (!isSplitTable)
                {
                    return await Context.Deleteable(entities.ToList()).ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Deleteable(entities.ToList()).SplitTable().ExecuteCommandAsync();
                }
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> RemoveAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {

            var isSplitTable = ClassHelper.IsSplitTable<IOrgAggregateRoot>();
            if (!isSplitTable)
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
            else
            {
                if (null != OrgWhere)
                {
                    return await Context.Deleteable(predicate).Where(OrgWhere).SplitTable().ExecuteCommandAsync();
                }
                else
                {
                    return await Context.Deleteable(predicate).SplitTable().ExecuteCommandAsync();
                }
            }
        }
    }
}