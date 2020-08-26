#region Licenses
/*MIT License
Copyright(c) 2020
Robert Garrison

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion
#region Using Statements
using ADO.Net.Client.Tests.Common;
using Moq;
using NUnit.Framework;
using System.Data;
using System.Data.Common;
using System.Linq;
#endregion

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
        #region Setup/Teardown        
        /// <summary>
        /// Called when [time setup].
        /// </summary>
        [OneTimeSetUp]
        public override void OneTimeSetup()
        {
            DbProviderFactories.RegisterFactory("ADO.Net.Client.Tests.Common", CustomDbProviderFactory.Instance);

            //For regular .NET framework the driver must be installed in the Global Assembly Cache
            DataTable table = DbProviderFactories.GetFactoryClasses();
            DataRow row = (from a in table.Rows.Cast<DataRow>()
                           where a.ItemArray[2].ToString() == "ADO.Net.Client.Tests.Common"
                           select a).FirstOrDefault();
            _factory = new DbObjectFactory(row, _formatter.Object);
        }
        #endregion
    }
}