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
        protected ISqlSugarClient Context { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected SqlSugarSqlRepository(ISqlSugarClient context)
        {
            Context = context;
        }

        /// <summary>
        /// sql语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<T> SqlQuery<T>(string sql) where T : class, new()
        {
            var context = (SqlSugarDbContext)Context;
            if (context.IsSingleDb)
            {
                return Context.SqlQueryable<T>(sql);
            }
            else
            {
                var dbContext = context.GetConnectionScopeWithAttr<T>();
                return dbContext.SqlQueryable<T>(sql);
            }
        }
    }
}