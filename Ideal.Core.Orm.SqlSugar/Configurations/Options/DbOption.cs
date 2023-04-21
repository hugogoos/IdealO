using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar.Configurations.Options
{
    /// <summary>
    /// 多库 配置
    /// </summary>
    public class DbOption
    {
        /// <summary>
        /// 多库唯一标识
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// 多库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 多库数据库类型
        /// </summary>
        public DbType DbType { get; set; }
    }
}
