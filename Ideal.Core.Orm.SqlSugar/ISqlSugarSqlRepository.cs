using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// Sql仓储接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface ISqlSugarSqlRepository<T> where T : class, new()
    {
        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ISugarQueryable<T> SqlQuery(string sql);
    }
}
