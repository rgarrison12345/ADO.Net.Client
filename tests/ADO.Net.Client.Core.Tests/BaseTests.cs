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
using ADO.Net.Client.Annotations;
using ADO.Net.Client.Tests.Common;
using ADO.Net.Client.Tests.Common.Models;
using Bogus;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
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
        #endregion
        #region Setup/Teardown     
        public abstract void Setup();
        #endregion
        #region Basic Tests
#if ADVANCED_FEATURES        
        /// <summary>
        /// Determines whether this instance [can create command builder].
        /// </summary>
        [Test]
        public void CanCreateCommandBuilder()
        {
            Assert.IsTrue(_factory.CanCreateCommandBuilder);
        }
        /// <summary>
        /// Determines whether this instance [can create database data adapter].
        /// </summary>
        [Test]
        public void CanCreateDbDataAdapter()
        {
            Assert.IsTrue(_factory.CanCreateDataAdapter);
        }
#endif
        /// <summary>
        /// Determines whether this instance [can create database data source enumerator].
        /// </summary>
        [Test]
        public void CreateDbDataSourceEnumerator()
        {
            DbDataSourceEnumerator dbDataSource = _factory.GetDataSourceEnumerator();

            Assert.IsNotNull(dbDataSource);
            Assert.IsInstanceOf(typeof(CustomDbDataSourceEnumerator), dbDataSource);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateDbDataAdapter()
        {
            DbDataAdapter dbDataAdapter = _factory.GetDbDataAdapter();

            Assert.IsNotNull(dbDataAdapter);
            Assert.IsInstanceOf(typeof(CustomDataAdapter), dbDataAdapter);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateDbConnection()
        {
            DbConnection connection = _factory.GetDbConnection();

            Assert.IsNotNull(connection);
            Assert.IsInstanceOf(typeof(CustomDbConnection), connection);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateConnectionStringBuilder()
        {
            DbConnectionStringBuilder builder = _factory.GetDbConnectionStringBuilder();

            Assert.IsNotNull(builder);
            Assert.IsInstanceOf(typeof(CustomConnectionStringBuilder), builder);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateCommandBuilder()
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
        public void CreateDbCommand()
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
            int commandTimeout = _faker.Random.Int();
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
        public void DbCommandTimeoutConnectionSame()
        {
            int commandTimeout = _faker.Random.Int();
            CustomDbConnection connection = new CustomDbConnection() { ConnectionString = _connectionString };
            DbCommand command = _factory.GetDbCommand(connection, commandTimeout);

            Assert.IsNotNull(command);
            Assert.AreEqual(commandTimeout, command.CommandTimeout);
            Assert.AreEqual(connection, command.Connection);
            Assert.IsInstanceOf(typeof(CustomDbConnection), command.Connection);
            Assert.IsInstanceOf(typeof(CustomDbCommand), command);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbCommandTests")]
        public void DbCommandTimeoutConnectionTransactionSame()
        {
            int commandTimeout = _faker.Random.Int();
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
            int commandTimeout = _faker.Random.Int();
            string queryText = "Select * From Users";
            CustomDbConnection connection = new CustomDbConnection() { ConnectionString = _connectionString };
            CustomDbTransaction transaction = connection.BeginTransaction() as CustomDbTransaction;
            CommandType type = _faker.PickRandom<CommandType>();
            DbCommand command = _factory.GetDbCommand(type, queryText, connection, null, commandTimeout, transaction);

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
            int commandTimeout = _faker.Random.Int();
            string queryText = "Select * From Users";
            CustomDbConnection connection = new CustomDbConnection() { ConnectionString = _connectionString };
            CustomDbTransaction transaction = connection.BeginTransaction() as CustomDbTransaction;
            CommandType type = _faker.PickRandom<CommandType>();
            DbCommand command = _factory.GetDbCommand(type, queryText, connection, parameters, commandTimeout, transaction);

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
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbParameterTests")]
        public void CreateDbParameter()
        {
            DbParameter parameter = _factory.GetDbParameter();

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
        }
        #endregion
    }
}