namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 支持软删除
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 软删除标志
        /// </summary>
        bool IsDeleted { get; set; }
    }
}