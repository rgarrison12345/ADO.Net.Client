using ADO.Net.Client.Tests.Common;
using NUnit.Framework;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseTests" />
    [TestFixture]
    [Category("DataRowTests")]
    public class DataRowTests : BaseTests
    {       
        /// <summary>
        /// Called when [time setup].
        /// </summary>
        [SetUp]
        public override void Setup()
        {
            DbProviderFactories.RegisterFactory(_commonNamespace, CustomDbProviderFactory.Instance);

            //For regular .NET framework the driver must be installed in the Global Assembly Cache
            DataTable table = DbProviderFactories.GetFactoryClasses();
            DataRow row = (from a in table.Rows.Cast<DataRow>()
                           where a.ItemArray[2].ToString() == _commonNamespace
                           select a).FirstOrDefault();
            _factory = new DbObjectFactory(row);
        }
    }
}