namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IAggregateRoot<TKey> : IEntity<TKey>
    {
    }
}