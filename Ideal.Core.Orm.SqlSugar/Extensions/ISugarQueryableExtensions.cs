using Ideal.Core.Common.Paging;
using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar.Extensions
{
    /// <summary>
    /// 分页查询扩展类
    /// </summary>
    public static class ISugarQueryableExtensions
    {
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="key">实体的主键</param>
        /// <returns>返回实体结果的异步任务</returns>
        public static async Task<T> FindByIdAsync<T, TKey>(this ISugarQueryable<T> source, TKey key) where T : class, IAggregateRoot<TKey>, new()
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.With(SqlWith.NoLock).InSingleAsync(key);
            }
            else
            {
                return await source.With(SqlWith.NoLock).Where(t => t.Id.Equals(key)).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        /// <summary>
        /// 查找第一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns>返回实体结果的异步任务</returns>
        public static async Task<T> FirstOrDefaultAsync<T>(this ISugarQueryable<T> source)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.With(SqlWith.NoLock).FirstAsync();
            }
            else
            {
                return await source.With(SqlWith.NoLock).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        /// <summary>
        /// 根据条件查找第一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体结果的异步任务</returns>
        public static async Task<T> FirstOrDefaultAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.With(SqlWith.NoLock).FirstAsync(predicate);
            }
            else
            {
                return await source.Where(predicate).With(SqlWith.NoLock).SplitTable(tabs => tabs).FirstAsync();
            }
        }

        /// <summary>
        /// 查找所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns>返回所有实体的列表的异步任务</returns>
        public static async Task<IEnumerable<T>> FindAllAsync<T>(this ISugarQueryable<T> source)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.ToListAsync();
            }
            else
            {
                return await source.SplitTable(tabs => tabs).ToListAsync();
            }
        }

        /// <summary>
        /// 查找所有分表实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回所有分表实体的列表的异步任务</returns>
        public static async Task<IEnumerable<T>> FindAllAsync<T>(this ISugarQueryable<T> source, DateTime startTime, DateTime endTime)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (isSplitTable)
            {
                return await source.SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await source.ToListAsync();
            }
        }

        /// <summary>
        /// 查找满足条件的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体列表结果的异步任务</returns>
        public static async Task<IEnumerable<T>> FindAllAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.Where(predicate).ToListAsync();
            }
            else
            {
                return await source.Where(predicate).SplitTable(tabs => tabs).ToListAsync();
            }
        }

        /// <summary>
        /// 查找满足条件的分表实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>返回实体列表结果的异步任务</returns>
        public static async Task<IEnumerable<T>> FindAllAsync<T>(this ISugarQueryable<T> source, DateTime startTime, DateTime endTime, Expression<Func<T, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (isSplitTable)
            {
                return await source.Where(predicate).SplitTable(startTime, endTime).ToListAsync();
            }
            else
            {
                return await source.Where(predicate).ToListAsync();
            }
        }

        /// <summary>
        /// 分页查找所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        public static async Task<IPagedList<T>> PagedFindAllAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, object>> orderByKeySelector, OrderByMode orderByType, Pager pager) where T : class
        {
            var totalCount = new RefAsync<int>();
            var query = source;

            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<T>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 分页查找所有分表实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表结果</returns>
        public static async Task<IPagedList<T>> PagedFindAllAsync<T>(this ISugarQueryable<T> source, DateTime startTime, DateTime endTime, Expression<Func<T, object>> orderByKeySelector, OrderByMode orderByType, Pager pager) where T : class
        {
            var totalCount = new RefAsync<int>();
            var query = source;

            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<T>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 分页查找满足条件的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">条件谓词</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        public static async Task<IPagedList<T>> PagedFindAllAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByKeySelector, OrderByMode orderByType, Pager pager) where T : class
        {
            var totalCount = new RefAsync<int>();
            var query = source.Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (isSplitTable)
            {
                query = query.SplitTable(tabs => tabs);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<T>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 分页查找满足条件的分表实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="predicate">条件谓词</param>
        /// <param name="orderByKeySelector">选择用于分页前排序的键</param>
        /// <param name="orderByType">排序类型</param>
        /// <param name="pager">分页器</param>
        /// <returns>分页的实体列表</returns>
        public static async Task<IPagedList<T>> PagedFindAllAsync<T>(this ISugarQueryable<T> source, DateTime startTime, DateTime endTime, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByKeySelector, OrderByMode orderByType, Pager pager) where T : class
        {
            var totalCount = new RefAsync<int>();
            var query = source.Where(predicate);

            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (isSplitTable)
            {
                query = query.SplitTable(startTime, endTime);
            }

            var page = await query.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pager.PageIndex, pager.PageSize, totalCount);
            var result = new PagedList<T>()
            {
                PageIndex = pager.PageIndex,
                PageSize = pager.PageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 判断是否存在指定主键的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="key">实体的主键</param>
        /// <returns>返回是否存在的异步任务</returns>
        public static async Task<bool> ExistsAsync<T, TKey>(this ISugarQueryable<T> source, TKey key) where T : class, IAggregateRoot<TKey>, new()
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.AnyAsync(entity => entity.Id.Equals(key));
            }
            else
            {
                return await source.Where(entity => entity.Id.Equals(key)).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        public static async Task<bool> ExistsAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.AnyAsync(predicate);
            }
            else
            {
                return await source.Where(predicate).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        /// <summary>
        /// 判断是否存在实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns>是否存在</returns>
        public static async Task<bool> AnyAsync<T>(this ISugarQueryable<T> source)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.AnyAsync();
            }
            else
            {
                return await source.SplitTable(tabs => tabs).AnyAsync();
            }
        }

        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        public static async Task<bool> AnyAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.AnyAsync(predicate);
            }
            else
            {
                return await source.Where(predicate).SplitTable(tabs => tabs).AnyAsync();
            }
        }

        /// <summary>
        /// 计算实体个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <returns>条数</returns>
        public static async Task<int> CountAsync<T>(this ISugarQueryable<T> source)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.CountAsync();
            }
            else
            {
                return await source.SplitTable(tabs => tabs).CountAsync();
            }
        }

        /// <summary>
        /// 计算满足条件的实体个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">条件谓词</param>
        /// <returns>是否存在</returns>
        public static async Task<int> CountAsync<T>(this ISugarQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            var isSplitTable = ClassHelper.IsSplitTable<T>();
            if (!isSplitTable)
            {
                return await source.CountAsync(predicate);
            }
            else
            {
                return await source.Where(predicate).SplitTable(tabs => tabs).CountAsync();
            }
        }
    }
}
