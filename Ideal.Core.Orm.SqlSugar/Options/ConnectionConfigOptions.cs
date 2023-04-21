using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar.Options
{
    /// <summary>
    /// SqlSugar配置项创建
    /// </summary>
    public class ConnectionConfigOptions : List<ConnectionConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ConnectionConfigOptions(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
