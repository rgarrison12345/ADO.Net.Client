using System.Data.Common;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbProviderFactory" />
    public class CustomDbProviderFactory : DbProviderFactory
    {
        public static CustomDbProviderFactory Instance = new CustomDbProviderFactory();

#if ADVANCED_FEATURES
        public override bool CanCreateCommandBuilder => base.CanCreateCommandBuilder;
        public override bool CanCreateDataAdapter => base.CanCreateDataAdapter;
#endif
        private CustomDbProviderFactory()
        {

        }
        
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new CustomDbCommandBuilder();
        }
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new CustomConnectionStringBuilder();
        }
        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return new CustomDbDataSourceEnumerator();
        }
        public override DbDataAdapter CreateDataAdapter()
        {
            return new CustomDataAdapter();
        }
        public override DbConnection CreateConnection()
        {
            return new CustomDbConnection();
        }
        public override DbCommand CreateCommand()
        {
            return new CustomDbCommand();
        }
        public override DbParameter CreateParameter()
        {
            return new CustomDbParameter();
        }

#if NET7_0_OR_GREATER
        public override DbDataSource CreateDataSource(string connectionString)
        {
            return new CustomDbDataSource();
        }
#endif
    }
}