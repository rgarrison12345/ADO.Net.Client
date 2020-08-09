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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
#endregion

namespace ADO.Net.Client.Implementation.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class QueryBuilderTests 
    {
        #region Fields/Properties
        private readonly IDbObjectFactory _factory;
        private readonly Faker _faker = new Faker();
        private IQueryBuilder _builder;
        #endregion
        #region Constructors
        public QueryBuilderTests()
        {
            _factory = new DbObjectFactory(MySqlConnectorFactory.Instance);
        }
        #endregion
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _builder = new QueryBuilder(_factory);
        }
        #endregion
        #region Tests    
        [Test]
        [Category("Build")]
        public void CanBuildSQLQuery()
        {
            string queryString = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool clearContents = _faker.Random.Bool();
            bool prepareQuery = _faker.Random.Bool();
            CommandType commandType = _faker.PickRandom<CommandType>();
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new MySqlParameter() { ParameterName = "@Param3" },
                new MySqlParameter() { ParameterName = "@Param2" },
                new MySqlParameter() { ParameterName = "@Param1" }
            };

            _builder.Append(queryString);
            _builder.AddParameterRange(parameters);

            ISqlQuery query = _builder.CreateSQLQuery(commandType, commandTimeout, prepareQuery, clearContents);

            Assert.IsNotNull(query);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(queryString, query.QueryText);
            Assert.AreEqual(prepareQuery, query.ShouldBePrepared);
            Assert.AreEqual(commandType, query.QueryType);
            Assert.IsTrue(_builder.Parameters.Count() == ((clearContents == false) ? parameters.Count : 0));
            Assert.IsTrue(_builder.QueryText == ((clearContents == false) ? queryString : string.Empty));
        }
        /// <summary>
        /// Containtses the parameter false.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterFalse()
        {
            MySqlParameter param = new MySqlParameter() { ParameterName = "Param", Value = 12321, DbType = DbType.Int32 };

            _builder.AddParameter(param);

            Assert.IsFalse(_builder.Contains(new MySqlParameter()));
        }
        /// <summary>
        /// Determines whether [contains parameter true].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterTrue()
        {
            MySqlParameter param = new MySqlParameter() { ParameterName = "Param", Value = 12321, DbType = DbType.Int32 };

            _builder.AddParameter(param);

            Assert.IsTrue(_builder.Contains(param));
        }
        /// <summary>
        /// Determines whether [contains parameter name false].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterNameFalse()
        {
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new MySqlParameter() { ParameterName = "@Param3" },
                new MySqlParameter() { ParameterName = "@Param2" },
                new MySqlParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            Assert.That(_builder.Contains("@Param4") == false);
        }
        [Test]
        [Category("Parameters")]
        public void ContainsParameterNameTrue()
        {
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new MySqlParameter() { ParameterName = "@Param3" },
                new MySqlParameter() { ParameterName = "@Param2" },
                new MySqlParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            Assert.That(_builder.Contains("@Param1"));
            Assert.That(_builder.Contains("@Param3"));
            Assert.That(_builder.Contains("@Param2"));
        }
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterName()
        {
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "@Param1" };

            _builder.AddParameter(parameter);

            Assert.Throws<ArgumentException>(() => _builder.AddParameter(new MySqlParameter() { ParameterName = "@Param1" }));
        }
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterNamesInEnumerable()
        {
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new MySqlParameter() { ParameterName = "@Param1" },
                new MySqlParameter() { ParameterName = "@Param2" },
                new MySqlParameter() { ParameterName = "@Param1" }
            };

            Assert.Throws<ArgumentException>(() => _builder.AddParameterRange(parameters));
        }
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterNames()
        {
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "@Param1" };
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new MySqlParameter() { ParameterName = "@Param3" },
                new MySqlParameter() { ParameterName = "@Param2" },
                new MySqlParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameter(parameter);

            Assert.Throws<ArgumentException>(() => _builder.AddParameterRange(parameters));
        }
        /// <summary>
        /// Determines whether this instance [can append string].
        /// </summary>
        [Test]
        [Category("SQL")]
        public void CanAppendString()
        {
            string valueToAppend = "Value To Append";

            _builder.Append(valueToAppend);

            Assert.That(!string.IsNullOrWhiteSpace(_builder.QueryText));
            Assert.That(valueToAppend == _builder.QueryText);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanAppendStringAndParameter()
        {
            string valueToAppend = "Value To Append";
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "Param1" };

            _builder.Append(valueToAppend, parameter);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(!string.IsNullOrWhiteSpace(_builder.QueryText));
            Assert.That(valueToAppend == _builder.QueryText);
            Assert.That(_builder.Parameters.Count() == 1);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanAppendStringAndParameters()
        {
            string valueToAppend = "Value To Append";
            List<DbParameter> parameters = new List<DbParameter>() 
            { 
                new MySqlParameter() { ParameterName = "Param1" },
                new MySqlParameter() { ParameterName = "Param2" },
                new MySqlParameter() { ParameterName = "Param3" }
            };

            _builder.Append(valueToAppend, parameters);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(!string.IsNullOrWhiteSpace(_builder.QueryText));
            Assert.That(valueToAppend == _builder.QueryText);
            Assert.That(_builder.Parameters.Count() == 3);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanClearParameters()
        {
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new MySqlParameter() { ParameterName = "Param1" },
                new MySqlParameter() { ParameterName = "Param2" },
                new MySqlParameter() { ParameterName = "Param3" }
            };

            _builder.AddParameterRange(parameters);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 3);

            _builder.ClearParameters();

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 0);
        }
        /// <summary>
        /// Determines whether this instance [can clear SQL string].
        /// </summary>
        [Test]
        [Category("SQL")]
        public void CanClearSqlString()
        {
            _builder.Append("A value to append \n");
            _builder.Append("A second value to append");

            //Clear the sql string
            _builder.ClearSQL();

            Assert.That(string.IsNullOrWhiteSpace(_builder.QueryText) == true);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanFindParameterByName()
        {
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "@Param1", Value = 1};

            _builder.AddParameter(parameter);

            DbParameter param = _builder.GetParameter(parameter.ParameterName);

            Assert.IsNotNull(param);
            Assert.AreEqual(typeof(MySqlParameter), param.GetType());
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.That(param.ParameterName == parameter.ParameterName);
            Assert.That(param.Value == parameter.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanRemoveParameterByName()
        {
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "@Param1", Value = 1 };

            _builder.AddParameter(parameter);

            _builder.RemoveParameter(parameter.ParameterName);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Contains(parameter.ParameterName) == false);
            Assert.That(_builder.Parameters.Count() == 0);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanReplaceParameterByName()
        {
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "@Param1", Value = 1 };
            MySqlParameter newParam = new MySqlParameter() { ParameterName="@Param2", Value = "SomeValue" };
            
            _builder.AddParameter(parameter);

            _builder.ReplaceParameter(parameter.ParameterName, newParam);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.AreNotEqual(parameter, newParam);
            Assert.AreNotEqual(parameter.Value, newParam.Value);
            Assert.AreNotEqual(parameter.ParameterName, newParam.ParameterName);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanSetParameterValueByName()
        {
            MySqlParameter parameter = new MySqlParameter() { ParameterName = "@Param1", Value = 1 };

            _builder.AddParameter(parameter);
            _builder.SetParamaterValue(parameter.ParameterName, 333);
            DbParameter param = _builder.GetParameter(parameter.ParameterName);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.That(param.ParameterName == parameter.ParameterName);
            Assert.That(333 == (int)param.Value);
        }
        #endregion
    }
}