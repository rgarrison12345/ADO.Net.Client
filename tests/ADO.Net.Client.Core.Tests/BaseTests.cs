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
using Bogus;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// Base test class
    /// </summary>
    [TestFixture]
    [Category("Basic Tests")]
    public abstract class BaseTests
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        private readonly string _connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
        protected IDbObjectFactory _factory;
        protected Mock<IDbParameterFormatter> _formatter = new Mock<IDbParameterFormatter>();
        #endregion
        #region Setup/Teardown                
        /// <summary>
        /// Setups this instance.
        /// </summary>
        public abstract void Setup();
        #endregion
        #region Basic Tests
        /// <summary>
        /// Determines whether this instance [can create database data source enumerator].
        /// </summary>
        [Test]
        public void CanCreateDbDataSourceEnumerator()
        {
            DbDataSourceEnumerator dbDataSource = _factory.GetDataSourceEnumerator();

            Assert.IsNotNull(dbDataSource);
            Assert.IsInstanceOf(typeof(CustomDbDataSourceEnumerator), dbDataSource);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanCreateDbDataAdapter()
        {
            DbDataAdapter dbDataAdapter = _factory.GetDbDataAdapter();

            Assert.IsNotNull(dbDataAdapter);
            Assert.IsInstanceOf(typeof(CustomDataAdapter), dbDataAdapter);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanCreateDbConnection()
        {
            DbConnection connection = _factory.GetDbConnection();

            Assert.IsNotNull(connection);
            Assert.IsInstanceOf(typeof(CustomDbConnection), connection);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanCreateConnectionStringBuilder()
        {
            DbConnectionStringBuilder builder = _factory.GetDbConnectionStringBuilder();

            Assert.IsNotNull(builder);
            Assert.IsInstanceOf(typeof(CustomConnectionStringBuilder), builder);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanCreateCommandBuilder()
        {
            DbCommandBuilder builder = _factory.GetDbCommandBuilder();

            Assert.IsNotNull(builder);
            Assert.IsInstanceOf(typeof(CustomDbCommandBuilder), builder);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbCommandTests")]
        public void CanCreateDbCommand()
        {
            DbCommand command = _factory.GetDbCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOf(typeof(CustomDbCommand), command);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbCommandTests")]
        public void DbCommandTimeoutSame()
        {
            int commandTimeout = 10;
            DbCommand command = _factory.GetDbCommand(commandTimeout);

            Assert.IsNotNull(command);
            Assert.AreEqual(commandTimeout, command.CommandTimeout);
            Assert.IsInstanceOf(typeof(CustomDbCommand), command);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbCommandTests")]
        public void DbCommandTimeoutConnectionTransactionSame()
        {
            int commandTimeout = 10;
            CustomDbConnection connection = new CustomDbConnection() { ConnectionString = _connectionString };
            CustomDbTransaction transaction = connection.BeginTransaction() as CustomDbTransaction;
            DbCommand command = _factory.GetDbCommand(connection, transaction, commandTimeout);

            Assert.IsNotNull(command);
            Assert.AreEqual(commandTimeout, command.CommandTimeout);
            Assert.AreEqual(connection, command.Connection);
            Assert.AreEqual(transaction, command.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), command.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbConnection), command.Connection);
            Assert.IsInstanceOf(typeof(CustomDbCommand), command);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbCommandTests")]
        public void DbCommandTimeoutConnectionTransactionSameNullParameters()
        {
            int commandTimeout = 10;
            string queryText = "Select * From Users";
            CustomDbConnection connection = new CustomDbConnection() { ConnectionString = _connectionString };
            CustomDbTransaction transaction = connection.BeginTransaction() as CustomDbTransaction;
            CommandType type = _faker.PickRandom<CommandType>();
            DbCommand command = _factory.GetDbCommand(type, queryText, null, connection, commandTimeout, transaction);

            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Parameters);
            Assert.IsNotNull(command.Connection);
            Assert.IsNotNull(command.Transaction);
            Assert.IsNotNull(command.CommandText);
            Assert.AreEqual(queryText, command.CommandText);
            Assert.AreEqual(type, command.CommandType);
            Assert.AreEqual(commandTimeout, command.CommandTimeout);
            Assert.AreEqual(connection, command.Connection);
            Assert.AreEqual(transaction, command.Transaction);
            Assert.IsTrue(command.Parameters.Count == 0);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), command.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbConnection), command.Connection);
            Assert.IsInstanceOf(typeof(CustomDbCommand), command);
            Assert.IsInstanceOf(typeof(CustomDbParameterCollection), command.Parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbCommandTests")]
        public void DbCommandTimeoutConnectionTransactionParametersSame()
        {
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };
            int commandTimeout = 10;
            string queryText = "Select * From Users";
            CustomDbConnection connection = new CustomDbConnection() { ConnectionString = _connectionString };
            CustomDbTransaction transaction = connection.BeginTransaction() as CustomDbTransaction;
            CommandType type = _faker.PickRandom<CommandType>();
            DbCommand command = _factory.GetDbCommand(type, queryText, parameters, connection, commandTimeout, transaction);

            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Parameters);
            Assert.IsNotNull(command.Connection);
            Assert.IsNotNull(command.Transaction);
            Assert.IsNotNull(command.CommandText);
            Assert.AreEqual(queryText, command.CommandText);
            Assert.AreEqual(type, command.CommandType);
            Assert.AreEqual(commandTimeout, command.CommandTimeout);
            Assert.AreEqual(connection, command.Connection);
            Assert.AreEqual(transaction, command.Transaction);
            Assert.IsTrue(parameters.Count == command.Parameters.Count);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), command.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbConnection), command.Connection);
            Assert.IsInstanceOf(typeof(CustomDbCommand), command);
            Assert.IsInstanceOf(typeof(CustomDbParameterCollection), command.Parameters);

            //Assert all parameters are equal
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                DbParameter parameter = command.Parameters[i];

                Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
                Assert.AreEqual(parameters[i], parameter);
            }
        }
        [Test]
        [Category("DbParameterTests")]
        public void ThrowsArguementExceptionValueType()
        {
            Assert.Throws<ArgumentException>(() => _factory.GetDbParameters(_faker.Random.Int()));
        }
        [Test]
        [Category("DbParameterTests")]
        public void ThrowsArguementExceptionString()
        {
            Assert.Throws<ArgumentException>(() => _factory.GetDbParameters(_faker.Random.AlphaNumeric(1)));
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbParameterTests")]
        public void CanCreateDbParameter()
        {
            DbParameter parameter = _factory.GetDbParameter();

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbParameterTests")]
        public void CanCreateDbParameterNameValue()
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            int value = _faker.Random.Int();

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _factory.GetDbParameter(name, value);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(name, parameter.ParameterName);
            Assert.AreEqual(value, parameter.Value);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbParameterTests")]
        public void CanCreateDbParameterNameNullValue()
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            object value = null;

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(DBNull.Value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _factory.GetDbParameter(name, value);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(name, parameter.ParameterName);
            Assert.AreEqual(DBNull.Value, parameter.Value);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        [Test]
        [Category("DbParameterTests")]
        public void CanCreateParameterByDbTypeDirection()
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            int value = _faker.Random.Int();
            DbType dbType = _faker.PickRandom<DbType>();
            ParameterDirection direction = _faker.PickRandom<ParameterDirection>();

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _factory.GetDbParameter(name, value, dbType, direction);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(direction, parameter.Direction);
            Assert.AreEqual(dbType, parameter.DbType);
            Assert.AreEqual(name, parameter.ParameterName);
            Assert.AreEqual(value, parameter.Value);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
        }
        /// <summary>
        /// Adds the parameters database parameter.
        /// </summary>
        [Test]
        [Category("DbParameterTests")]
        public void AddParametersDbParameter()
        {
            CustomDbParameter param = new CustomDbParameter();

            IEnumerable<DbParameter> returned = _factory.GetDbParameters(param);

            Assert.IsTrue(returned.Count() == 1);
            Assert.AreEqual(returned.First(), param);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        [Test]
        [Category("DbParameterTests")]
        [TestCase(10)]
        [TestCase(null)]
        public void CanCreateVariableSizeParameter(int? size)
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            string value = _faker.Random.AlphaNumeric(40);
            DbType dbType = _faker.PickRandom<DbType>();
            ParameterDirection direction = _faker.PickRandom<ParameterDirection>();

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _factory.GetVariableSizeDbParameter(name, value, dbType, size, direction);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(size ?? 0, parameter.Size);
            Assert.AreEqual(direction, parameter.Direction);
            Assert.AreEqual(dbType, parameter.DbType);
            Assert.AreEqual(name, parameter.ParameterName);
            Assert.AreEqual(value, parameter.Value);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
        }
#if !NET45
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="precision"></param>
        [Test]
        [Category("DbParameterTests")]
        [TestCase(null, 10)]
        [TestCase(10, null)]
        public void CanCreateFixedSizeParameter(byte? scale, byte? precision)
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            int value = _faker.Random.Int();
            DbType dbType = _faker.PickRandom<DbType>();
            ParameterDirection direction = _faker.PickRandom<ParameterDirection>();

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _factory.GetFixedSizeDbParameter(name, value, dbType, scale, precision, direction);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(scale ?? 0, parameter.Scale);
            Assert.AreEqual(precision ?? 0, parameter.Precision);
            Assert.AreEqual(direction, parameter.Direction);
            Assert.AreEqual(dbType, parameter.DbType);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
        }
#endif
        #endregion
    }
}