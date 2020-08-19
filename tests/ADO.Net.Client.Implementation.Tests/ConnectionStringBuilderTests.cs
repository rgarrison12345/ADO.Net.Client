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
using MySqlConnector;
using NUnit.Framework;
#endregion

namespace ADO.Net.Client.Implementation.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConnectionStringBuilderTests
    {
        #region Fields/Properties
        private readonly string _connectionString = "Server=127.0.0.1;Database=AdventureWorks;Port=3306;UId=myUsername;Pwd=myPassword;";
        private MySqlConnectionStringBuilder _sqlBuilder;
        private ConnectionStringBuilder _builder;
        #endregion
        #region Setup/Teardown        
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
        #endregion
        #region Tests        
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
        #endregion
    }
}