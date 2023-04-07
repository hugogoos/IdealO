namespace Ideal.Core.Orm.Domain.Organization
{
    /// <summary>
    /// 支持机构的实体
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class OrgEntity<TKey> : Entity<TKey>, IOrganization
    {
        /// <summary>
        /// 机构标志
        /// </summary>
        public virtual string OrgId { get; set; }
    }
}