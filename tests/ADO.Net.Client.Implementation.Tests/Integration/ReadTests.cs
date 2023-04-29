using ADO.Net.Client.Core;
using Bogus;
using MySqlConnector;
using NUnit.Framework;

namespace ADO.Net.Client.Implementation.Tests.Integration
{
    [TestFixture]
    [Ignore("Not Complete")]
    public partial class ReadTests
    {
        private IConnectionManager _manager;
        private IDbObjectFactory _factory;
        private readonly Faker _faker = new Faker();
        private readonly string connectionString = "Server=localhost;Database=DB;Uid=myUsername;Pwd=myPassword;Port=3306";
       
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
    }
}
