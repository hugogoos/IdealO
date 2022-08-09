using System;

namespace Ideal.Core.Orm.Domain
{
    /// <summary>
    /// 支持审计
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        string UpdatedBy { get; set; }
    }
}