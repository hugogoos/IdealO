namespace Ideal.Core.Common.Paging
{
    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PagedList<TEntity> : IPagedList<TEntity> where TEntity : class
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 实体集合
        /// </summary>
        public IEnumerable<TEntity> Entities { get; set; } = new List<TEntity>();
    }
}