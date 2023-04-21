using Ideal.Core.Orm.SqlSugar.Configurations.Options;

namespace Ideal.Core.Orm.SqlSugar.Configurations
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public interface IConfigurationCenter
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// 数据库主从连接字符串
        /// </summary>
        MasterSlaveOption ConnectionStrings { get; }

        /// <summary>
        /// 数据库配置
        /// </summary>
        SqlSugarOptions SqlSugarOptions { get; }
    }
}
