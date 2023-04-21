using Ideal.Core.Orm.Domain;
using SkiaSharp;
using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SqlSugarSqlRepository
    {
        /// <summary>
        /// 
        /// </summary>
        protected IDbContext DbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        protected SqlSugarSqlRepository(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<T> SqlQuery<T>(string sql) where T : class, new()
        {
            var context = (SqlSugarDbContext)DbContext;
            var scopedContext = ((SqlSugarScope)context.ISqlSugarClient).ScopedContext;
            if (1 == context.ConnectionConfigs.Count)
            {
                var provider = new SqlSugarScopeProvider(scopedContext.Context);
                return provider.SqlQueryable<T>(sql);
            }
            else
            {
                var provider = scopedContext.GetConnectionScopeWithAttr<T>();
                return provider.SqlQueryable<T>(sql);
            }
        }
    }
}