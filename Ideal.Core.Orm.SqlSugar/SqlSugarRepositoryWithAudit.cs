using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarRepositoryWithAudit<TAggregateRoot, TKey> : SqlSugarRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, IAuditable, new()
    {
        protected SqlSugarRepositoryWithAudit(ISqlSugarClient context) : base(context)
        {
        }

        public override int Update(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.UpdatedTime = DateTime.Now;
                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

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

        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommand();
        }

        public virtual int UpdateColumns(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            return Context.Updateable<TAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommand();
        }

        public override int Save(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.UpdatedTime = DateTime.Now;
                return Context.Storageable(entity).ExecuteCommand();
            }

            return 0;
        }

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