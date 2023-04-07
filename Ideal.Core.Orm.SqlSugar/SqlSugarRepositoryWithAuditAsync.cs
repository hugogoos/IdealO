using Ideal.Core.Orm.Domain;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar
{
    public abstract partial class SqlSugarRepositoryWithAudit<TAggregateRoot, TKey> : SqlSugarRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, IAuditable, new()
    {
        public override async Task<int> UpdateAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.UpdatedTime = DateTime.Now;
                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> UpdateAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var es = entities.ToList();
                es.ForEach(entity =>
                {
                    entity.UpdatedTime = DateTime.Now;
                });

                return await Context.Updateable(es).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommandAsync();
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<TAggregateRoot, TAggregateRoot>> predicate)
        {
            return await Context.Updateable<TAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommandAsync();
        }

        public override async Task<int> SaveAsync(TAggregateRoot entity)
        {
            if (entity != null)
            {
                entity.UpdatedTime = DateTime.Now;
                return await Context.Storageable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> SaveAsync(IEnumerable<TAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                var es = entities.ToList();
                es.ForEach(entity =>
                {
                    entity.UpdatedTime = DateTime.Now;
                });

                return await Context.Storageable(es).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }
    }
}