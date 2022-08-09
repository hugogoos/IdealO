namespace Ideal.Core.Orm.Domain.Organization
{
    /// <summary>
    /// 机构聚合根
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IOrgAggregateRoot<TKey> : IAggregateRoot<TKey>, IOrganization
    {
    }
}