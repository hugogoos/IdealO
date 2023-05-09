using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract partial class SqlSugarRepositoryWithAudit<TAggregateRoot, TKey> : SqlSugarRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, IAuditable, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected SqlSugarRepositoryWithAudit(ISqlSugarClient context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Update(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.UpdatedTime = DateTime.Now;
                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public override int Update(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var es = entities.ToList();
                es.ForEach(entity =>
                {
                    entity.UpdatedTime = DateTime.Now;
                });

                return Context.Updateable(es).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override int UpdateColumns(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Save(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.UpdatedTime = DateTime.Now;
                return Context.Storageable(entity).ExecuteCommand();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public override int Save(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var es = entities.ToList();
                es.ForEach(entity =>
                {
                    entity.UpdatedTime = DateTime.Now;
                });

                return Context.Storageable(es).ExecuteCommand();
            }

            return 0;
        }
    }
}