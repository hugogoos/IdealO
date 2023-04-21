using SqlSugar;

namespace Ideal.Core.Orm.SqlSugar.UnitOfWorks
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly object unitOfWorkLock = new();
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        /// 
        /// </summary>
        private int _tranCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        public UnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
            _tranCount = 0;
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetDbClient()
        {
            // 必须要as，后边会用到切换数据库操作
            return _sqlSugarClient as SqlSugarScope;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTran()
        {
            lock (unitOfWorkLock)
            {
                _tranCount++;
                GetDbClient().BeginTran();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CommitTran()
        {
            lock (unitOfWorkLock)
            {
                _tranCount--;
                if (_tranCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RollbackTran()
        {
            lock (unitOfWorkLock)
            {
                _tranCount--;
                GetDbClient().RollbackTran();
            }
        }

    }

}
