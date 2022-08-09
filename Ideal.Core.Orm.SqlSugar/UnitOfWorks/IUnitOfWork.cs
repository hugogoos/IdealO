using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar.UnitOfWorks
{
    public interface IUnitOfWork
    {
        SqlSugarScope GetDbClient();

        void BeginTran();

        void CommitTran();

        void RollbackTran();
    }
}
