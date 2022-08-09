namespace Ideal.Core.Orm.Domain.Organization
{
    /// <summary>
    /// 支持机构软删除的实体
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class OrgSoftDeleteEntity<TKey> : OrgAuditableEntity<TKey>, ISoftDelete
    {
        /// <summary>
        /// 软删除标志
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}