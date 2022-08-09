namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IEntity<out TKey>
    {
        /// <summary>
        /// 实体标识
        /// </summary>
        TKey Id { get; }
    }
}