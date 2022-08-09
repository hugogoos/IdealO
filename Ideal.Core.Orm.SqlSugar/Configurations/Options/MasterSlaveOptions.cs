using System.Collections.Generic;

namespace Ideal.Core.Orm.SqlSugar.Configurations.Options
{
    /// <summary>
    /// 主从库 配置
    /// </summary>
    public class MasterSlaveOptions
    {
        /// <summary>
        /// 主库连接字符串
        /// </summary>
        public string Master { get; set; }

        /// <summary>
        /// 从库连接字符串
        /// </summary>
        public List<string> Slaves { get; set; }
    }
}
