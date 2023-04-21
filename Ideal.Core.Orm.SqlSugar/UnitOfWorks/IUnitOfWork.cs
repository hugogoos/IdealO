using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar.UnitOfWorks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SqlSugarScope GetDbClient();

        /// <summary>
        /// 
        /// </summary>
        void BeginTran();

        /// <summary>
        /// 
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 
        /// </summary>
        void RollbackTran();
    }
}
