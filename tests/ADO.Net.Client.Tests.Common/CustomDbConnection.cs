using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbConnection" />
    public class CustomDbConnection : DbConnection
    {
        private ConnectionState _state = ConnectionState.Open;
        protected override DbProviderFactory DbProviderFactory => CustomDbProviderFactory.Instance;
        public override string ConnectionString { get; set; }

        public override string Database => throw new System.NotImplementedException();

        public override string DataSource => throw new System.NotImplementedException();

        public override string ServerVersion => throw new System.NotImplementedException();

        public override ConnectionState State => _state;

        public override void ChangeDatabase(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }

        public override void Open()
        {
            throw new System.NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new CustomDbTransaction(this, isolationLevel);
        }
#if !NET462 && !NETSTANDARD2_0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override ValueTask<DbTransaction> BeginDbTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            return new ValueTask<DbTransaction>(new CustomDbTransaction(this, isolationLevel));
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DbCommand CreateDbCommand()
        {
            return new CustomDbCommand();
        }
        
        public CustomDbConnection()
        {

        }
        public CustomDbConnection(ConnectionState state)
        {
            _state = state;
        }
    }
}