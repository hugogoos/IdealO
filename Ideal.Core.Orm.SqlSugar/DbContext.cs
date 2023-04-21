using Ideal.Core.Orm.Domain;
using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// DB上下文
    /// </summary>
    public class SqlSugarDbContext : IDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public ISqlSugarClient ISqlSugarClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ConnectionConfig> ConnectionConfigs { get; set; }
    }
}
