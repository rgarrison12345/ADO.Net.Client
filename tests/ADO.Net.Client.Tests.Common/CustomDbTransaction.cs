using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbTransaction" />
    public class CustomDbTransaction : DbTransaction
    {
        public override IsolationLevel IsolationLevel { get; }

        protected override DbConnection DbConnection { get; }

        public CustomDbTransaction(DbConnection connection, IsolationLevel level)
        {
            DbConnection = connection;
            IsolationLevel = level;
        }
        public CustomDbTransaction()
        {

        }
        public override void Commit()
        {
            throw new System.NotImplementedException();
        }

        public override void Rollback()
        {
            throw new System.NotImplementedException();
        }
    }
}