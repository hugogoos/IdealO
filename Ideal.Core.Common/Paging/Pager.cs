namespace Ideal.Core.Common.Paging
{
    /// <summary>
    /// 分页器
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// 分页索引（默认为1）
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页大小（默认为10）
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}