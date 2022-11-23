using System.Data.Common;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbDataSource"/>
    public class CustomDbDataSource : DbDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        public override string ConnectionString { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DbConnection CreateDbConnection()
        {
            return new CustomDbConnection();
        }
    }
}
