using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract partial class OrgSqlSugarRepositoryWithAudit<IOrgAggregateRoot, TKey> : OrgSqlSugarRepository<IOrgAggregateRoot, TKey>
        where IOrgAggregateRoot : class, IOrgAggregateRoot<TKey>, IAuditable, new()
    {
        public virtual async Task<int> CreateAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                AddCreateUserInfo(entity);

                return await Context.Insertable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> CreateAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddCreateUserInfo(entities);

                return await Context.Insertable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> UpdateAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                AddUpdateUserInfo(entity);

                return await Context.Updateable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> UpdateAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddUpdateUserInfo(entities);

                return await Context.Updateable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var updatedBy = OrgContext?.GetUserIdAndName;

            if (null != OrgWhere)
            {
                return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now && t.UpdatedBy == updatedBy).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommandAsync();
            }
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<IOrgAggregateRoot, IOrgAggregateRoot>> predicate)
        {
            var updatedBy = OrgContext?.GetUserIdAndName;

            if (null != OrgWhere)
            {
                return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now && t.UpdatedBy == updatedBy).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommandAsync();
            }
            else
            {
                return await Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommandAsync();
            }
        }

        public override async Task<int> SaveAsync(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return await Task.FromResult(0);
                }

                AddUpdateUserInfo(entity);

                return await Context.Storageable(entity).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }

        public override async Task<int> SaveAsync(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddUpdateUserInfo(entities);

                return await Context.Storageable(entities.ToList()).ExecuteCommandAsync();
            }

            return await Task.FromResult(0);
        }
    }
}