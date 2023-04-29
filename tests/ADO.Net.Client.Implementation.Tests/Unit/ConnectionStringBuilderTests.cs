using MySqlConnector;
using NUnit.Framework;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConnectionStringBuilderTests
    {
        private readonly string _connectionString = "Server=127.0.0.1;Database=AdventureWorks;Port=3306;UId=myUsername;Pwd=myPassword;";
        private MySqlConnectionStringBuilder _sqlBuilder;
        private ConnectionStringBuilder _builder;
    
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _sqlBuilder = new MySqlConnectionStringBuilder();
            _sqlBuilder.ConnectionString = _connectionString;
            _builder = new ConnectionStringBuilder(_sqlBuilder);
        }
    
        /// <summary>
        /// Adds the connection string property.
        /// </summary>
        [Test]
        public void AddConnectionStringProperty()
        {
            _builder.AddConnectionStringProperty("SslMode", "Preferred");

            Assert.That(_builder.ConnectionString.EndsWith("SSL Mode=Preferred"));
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void RemoveConnectionStringProperty()
        {
            _builder.RemoveConnectionStringProperty("Port");

            Assert.That(_builder.ConnectionString.Contains("Port=3306;") == false);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetConnectionStringPropertyValueReturnsNull()
        {
            object value = _builder.GetConnectionStringPropertyValue("SSL Mode");

            Assert.IsNull(value);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetConnectionStringPropertyValue()
        {
            object value = _builder.GetConnectionStringPropertyValue("Port");

            Assert.AreEqual(3306.ToString(), value);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ClearConnectionString()
        {
            _builder.ClearConnectionString();

            Assert.That(string.IsNullOrWhiteSpace(_builder.ConnectionString));
        }
    }
}