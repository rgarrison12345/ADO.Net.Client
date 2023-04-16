using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbDataSourceEnumerator" />
    public class CustomDbDataSourceEnumerator : DbDataSourceEnumerator
    {
        public override DataTable GetDataSources()
        {
            throw new System.NotImplementedException();
        }
    }
}
