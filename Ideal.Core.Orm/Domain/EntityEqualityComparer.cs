namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 实体比较器
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class EntityEqualityComparer<TEntity, TKey> : EqualityComparer<TEntity> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(TEntity x, TEntity y)
        {
            if (x == null || y == null) { return false; }
            return EqualityComparer<TKey>.Default.Equals(x.Id, y.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int GetHashCode(TEntity obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}