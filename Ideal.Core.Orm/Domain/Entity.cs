namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 实体标识
        /// </summary>
        public virtual TKey Id { get; set; }
    }
}