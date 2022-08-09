namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 支持软删除的实体
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class SoftDeleteEntity<TKey> : AuditableEntity<TKey>, ISoftDelete
    {
        /// <summary>
        /// 软删除标志
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}