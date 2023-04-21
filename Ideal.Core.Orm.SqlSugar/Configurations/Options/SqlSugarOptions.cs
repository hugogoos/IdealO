namespace Ideal.Core.Orm.SqlSugar.Configurations.Options
{
    /// <summary>
    /// 主从库 配置
    /// </summary>
    public class SqlSugarOptions
    {
        /// <summary>
        /// 多库
        /// </summary>
        public DbOption SingleDbOption { get; set; }

        /// <summary>
        /// 多库
        /// </summary>
        public DbOption[] MultiDbOptions { get; set; }

        /// <summary>
        /// 主从
        /// </summary>
        public MasterSlaveOption MasterSlaveOption { get; set; }
    }
}
