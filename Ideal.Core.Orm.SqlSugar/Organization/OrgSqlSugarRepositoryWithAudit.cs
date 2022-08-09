using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract class OrgSqlSugarRepositoryWithAudit<IOrgAggregateRoot, TKey> : OrgSqlSugarRepository<IOrgAggregateRoot, TKey>
        where IOrgAggregateRoot : class, IOrgAggregateRoot<TKey>, IAuditable, new()
    {
        protected OrgSqlSugarRepositoryWithAudit(ISqlSugarClient context, OrgContext orgContext) : base(context, orgContext)
        {
        }

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

        protected void AddCreateUserInfo(IOrgAggregateRoot entity)
        {
            var userInfo = OrgContext?.GetUserIdAndName;
            if (!string.IsNullOrWhiteSpace(userInfo))
            {
                if (string.IsNullOrWhiteSpace(entity.CreatedBy))
                {
                    entity.CreatedBy = OrgContext?.GetUserIdAndName;
                }
                if (string.IsNullOrWhiteSpace(entity.UpdatedBy))
                {
                    entity.UpdatedBy = OrgContext?.GetUserIdAndName;
                }
            }

            if (string.IsNullOrWhiteSpace(entity.OrganizationId))
            {
                entity.OrganizationId = OrgContext?.CurrentOrganizationId;
            }

            entity.CreatedTime = DateTime.Now;
            entity.UpdatedTime = DateTime.Now;
        }

        protected void AddCreateUserInfo(IEnumerable<IOrgAggregateRoot> entities)
        {
            foreach (var entity in entities)
            {
                AddCreateUserInfo(entity);
            }
        }

        protected void AddUpdateUserInfo(IOrgAggregateRoot entity)
        {
            if (string.IsNullOrWhiteSpace(entity.UpdatedBy) && !string.IsNullOrWhiteSpace(OrgContext?.GetUserIdAndName))
            {
                entity.UpdatedBy = OrgContext?.GetUserIdAndName;
            }

            entity.UpdatedTime = DateTime.Now;
        }

        protected void AddUpdateUserInfo(IEnumerable<IOrgAggregateRoot> entities)
        {
            foreach (var entity in entities)
            {
                AddUpdateUserInfo(entity);
            }
        }
    }
}