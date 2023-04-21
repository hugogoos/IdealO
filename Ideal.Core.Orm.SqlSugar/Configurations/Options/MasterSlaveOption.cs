using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar.Configurations.Options
{
    /// <summary>
    /// 主从库 配置
    /// </summary>
    public class MasterSlaveOption
    {
        /// <summary>
        /// 多库数据库类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 主库连接字符串
        /// </summary>
        public string MasterConnectionString { get; set; }

        /// <summary>
        /// 从库连接字符串
        /// </summary>
        public List<string> SlaveConnectionStrings { get; set; }
    }
}
