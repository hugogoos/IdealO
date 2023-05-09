using Ideal.Core.Orm.Domain;
using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// DB上下文
    /// </summary>
    public class SqlSugarDbContext : SqlSugarScope
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public SqlSugarDbContext(ConnectionConfig config) : base(config)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configs"></param>
        public SqlSugarDbContext(List<ConnectionConfig> configs) : base(configs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="configAction"></param>
        public SqlSugarDbContext(ConnectionConfig config, Action<SqlSugarClient> configAction) : base(config, configAction)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configs"></param>
        /// <param name="configAction"></param>
        public SqlSugarDbContext(List<ConnectionConfig> configs, Action<SqlSugarClient> configAction) : base(configs, configAction)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsSingleDb { get; set; } = true;
    }
}
