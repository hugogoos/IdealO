namespace Ideal.Core.Common.Paging
{
    /// <summary>
    /// 分页接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPagedList<TEntity> where TEntity : class
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// 实体集合
        /// </summary>
        IEnumerable<TEntity> Entities { get; set; }
    }
}