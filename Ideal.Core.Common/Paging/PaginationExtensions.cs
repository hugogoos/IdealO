namespace Ideal.Core.Common.Paging
{
    /// <summary>
    /// 分页查询扩展类
    /// </summary>
    public static class PaginationExtensions
    {
        #region 内存分页

        /// <summary>
        /// 返回对象分页列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataSource">已排序的数据源</param>
        /// <param name="pageIndex">页码，1开始</param>
        /// <param name="pageSize">页条数</param>
        /// <returns>对象分页列表</returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> dataSource, int pageIndex, int pageSize)
            where T : class
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var pagedList = new PagedList<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = dataSource.Count(),
                Entities = dataSource.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };

            return pagedList;
        }

        /// <summary>
        /// 返回对象分页列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dataSource">已排序的数据源</param>
        /// <param name="pager">分页器对象；当为空时，分页器取默认值</param>
        /// <returns>对象分页列表</returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> dataSource, Pager pager) where T : class
        {
            pager ??= new Pager();
            return dataSource.ToPagedList(pager.PageIndex, pager.PageSize);
        }

        #endregion
    }
}
