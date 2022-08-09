using Ideal.Core.Common.Paging;
using SqlSugar;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ideal.Core.Orm.SqlSugar.Extensions
{
    /// <summary>
    /// 分页查询扩展类
    /// </summary>
    public static class PaginationExtensions
    {
        #region IQueryable分页

        /// <summary>
        /// 返回对象分页列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataSource">已排序的数据源</param>
        /// <param name="pageIndex">页码，1开始</param>
        /// <param name="pageSize">页条数</param>
        /// <returns>对象分页列表</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this ISugarQueryable<T> dataSource, int pageIndex, int pageSize)
            where T : class
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var totalCount = new RefAsync<int>();
            var page = await dataSource.ToPageListAsync(pageIndex, pageSize, totalCount);
            var result = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 返回对象分页列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataSource">已排序的数据源</param>
        /// <param name="pager">分页器对象；当为空时，分页器取默认值</param>
        /// <returns>对象分页列表</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this ISugarQueryable<T> dataSource, Pager pager) where T : class
        {
            pager ??= new Pager();
            return await dataSource.ToPagedListAsync(pager.PageIndex, pager.PageSize);
        }

        /// <summary>
        /// 返回对象分页列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataSource">已排序的数据源</param>
        /// <param name="pageIndex">页码，1开始</param>
        /// <param name="pageSize">页条数</param>
        /// <param name="orderByKeySelector">排序字段</param>
        /// <param name="orderByType">排序类型</param>
        /// <returns>对象分页列表</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this ISugarQueryable<T> dataSource, int pageIndex, int pageSize, Expression<Func<T, object>> orderByKeySelector, OrderByMode orderByType)
            where T : class
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var totalCount = new RefAsync<int>();
            var page = await dataSource.OrderBy(orderByKeySelector, orderByType == OrderByMode.Asc ? OrderByType.Asc : OrderByType.Desc).ToPageListAsync(pageIndex, pageSize, totalCount);
            var result = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount.Value,
                Entities = page
            };
            return result;
        }

        /// <summary>
        /// 返回对象分页列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataSource">已排序的数据源</param>
        /// <param name="pager">分页器对象；当为空时，分页器取默认值</param>
        /// <param name="orderByKeySelector">排序字段</param>
        /// <param name="orderByType">排序类型</param>
        /// <returns>对象分页列表</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this ISugarQueryable<T> dataSource, Pager pager, Expression<Func<T, object>> orderByKeySelector, OrderByMode orderByType) where T : class
        {
            pager ??= new Pager();
            return await dataSource.ToPagedListAsync(pager.PageIndex, pager.PageSize, orderByKeySelector, orderByType);
        }
        #endregion
    }
}
