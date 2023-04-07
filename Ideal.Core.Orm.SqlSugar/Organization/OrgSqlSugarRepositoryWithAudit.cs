using Ideal.Core.Orm.Domain;
using Ideal.Core.Orm.Domain.Organization;
using SqlSugar;
using System.Linq.Expressions;

namespace Ideal.Core.Orm.SqlSugar.Organization
{
    public abstract partial class OrgSqlSugarRepositoryWithAudit<IOrgAggregateRoot, TKey> : OrgSqlSugarRepository<IOrgAggregateRoot, TKey>
        where IOrgAggregateRoot : class, IOrgAggregateRoot<TKey>, IAuditable, new()
    {
        protected OrgSqlSugarRepositoryWithAudit(ISqlSugarClient context, OrgContext orgContext) : base(context, orgContext)
        {
        }

        public virtual int Create(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                AddCreateUserInfo(entity);

                return Context.Insertable(entity).ExecuteCommand();
            }

            return 0;
        }

        public virtual int Create(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddCreateUserInfo(entities);

                return Context.Insertable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public override int Update(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                AddUpdateUserInfo(entity);

                return Context.Updateable(entity).ExecuteCommand();
            }

            return 0;
        }

        public override int Update(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddUpdateUserInfo(entities);


                return Context.Updateable(entities.ToList()).ExecuteCommand();
            }

            return 0;
        }

        public virtual int UpdateColumns(Expression<Func<IOrgAggregateRoot, bool>> predicate)
        {
            var updatedBy = OrgContext?.GetUserIdAndName;

            if (null != OrgWhere)
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now && t.UpdatedBy == updatedBy).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommand();
            }
            else
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommand();
            }
        }

        public virtual int UpdateColumns(Expression<Func<IOrgAggregateRoot, IOrgAggregateRoot>> predicate)
        {
            var updatedBy = OrgContext?.GetUserIdAndName;

            if (null != OrgWhere)
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now && t.UpdatedBy == updatedBy).Where(t => t.Id != null).Where(OrgWhere).ExecuteCommand();
            }
            else
            {
                return Context.Updateable<IOrgAggregateRoot>().SetColumns(predicate).SetColumns(t => t.UpdatedTime == DateTime.Now).Where(t => t.Id != null).ExecuteCommand();
            }
        }

        public override int Save(IOrgAggregateRoot entity)
        {
            if (entity != null)
            {
                var isIllegalOrg = IsIllegalOrg(entity);
                if (isIllegalOrg)
                {
                    return 0;
                }

                AddUpdateUserInfo(entity);

                return Context.Storageable(entity).ExecuteCommand();
            }

            return 0;
        }

        public override int Save(IEnumerable<IOrgAggregateRoot> entities)
        {
            if (entities != null && entities.Any())
            {
                RemoveIllegalOrgs(entities);

                AddUpdateUserInfo(entities);

                return Context.Storageable(entities.ToList()).ExecuteCommand();
            }

            return 0;
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

            if (string.IsNullOrWhiteSpace(entity.OrgId))
            {
                entity.OrgId = OrgContext?.CurrentOrgId;
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