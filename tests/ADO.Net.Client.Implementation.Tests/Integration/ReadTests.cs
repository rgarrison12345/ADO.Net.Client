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
using ADO.Net.Client.Core;
using Bogus;
using MySqlConnector;
using NUnit.Framework;
using System.Data;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Implementation.Tests.Integration
{
    [TestFixture]
    [Ignore("Not Complete")]
    public partial class ReadTests
    {
        #region Fields/Properties
        private IConnectionManager _manager;
        private IDbObjectFactory _factory;
        private readonly Faker _faker = new Faker();
        private readonly string connectionString = "Server=localhost;Database=DB;Uid=myUsername;Pwd=myPassword;Port=3306";
        #endregion
        #region Setup/Teardown        
        /// <summary>
        /// Called when [time setup].
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            string query = "";

            //await new SqlExecutor(_factory, _manager).ExecuteNonQueryAsync(query, CommandType.Text, null);
        }
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _manager = new ConnectionManager(new MySqlConnection(connectionString));
            _factory = new DbObjectFactory(MySqlConnectorFactory.Instance);

            _manager.Connection.Open();
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            _manager.Connection.Close();
        }
        #endregion
    }
}
