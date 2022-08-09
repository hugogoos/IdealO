using System;

namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 支持审计的实体
    /// </summary>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class AuditableEntity<TKey> : Entity<TKey>, IAuditable
    {
        public AuditableEntity()
        {
            var currentTime = DateTime.Now;
            CreatedTime = currentTime;
            UpdatedTime = currentTime;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public virtual string UpdatedBy { get; set; }
    }
}