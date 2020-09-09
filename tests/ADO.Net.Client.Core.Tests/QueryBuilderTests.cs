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
    /// 
    /// </summary>
    [TestFixture]
    public class QueryBuilderTests 
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterBuilder> paramBuilder = new Mock<IDbParameterBuilder>();
        private IQueryBuilder _builder;
        #endregion
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilderTests"/> class.
        /// </summary>
        public QueryBuilderTests()
        {
        }
        #endregion
        #region Setup/Teardown        
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            paramBuilder = new Mock<IDbParameterBuilder>();
            _builder = new QueryBuilder(paramBuilder.Object);
        }
        #endregion
        #region Tests         
        /// <summary>
        /// Containtses the parameter false.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterFalse()
        {
            CustomDbParameter param = new CustomDbParameter() { ParameterName = "Param", Value = 12321, DbType = DbType.Int32 };

            _builder.AddParameter(param);

            Assert.IsFalse(_builder.Contains(new CustomDbParameter()));
        }
        /// <summary>
        /// Determines whether [contains parameter true].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterTrue()
        {
            CustomDbParameter param = new CustomDbParameter() { ParameterName = "Param", Value = 12321, DbType = DbType.Int32 };

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
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            Assert.That(_builder.Contains("@Param4") == false);
        }
        /// <summary>
        /// Determines whether [contains parameter name true].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterNameTrue()
        {
            List<DbParameter> parameters = new List<DbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            Assert.That(_builder.Contains("@Param1"));
            Assert.That(_builder.Contains("@Param3"));
            Assert.That(_builder.Contains("@Param2"));
        }
        /// <summary>
        /// Rejectses the name of the duplicate parameter.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterName()
        {
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "@Param1" };

            _builder.AddParameter(parameter);

            Assert.Throws<ArgumentException>(() => _builder.AddParameter(new CustomDbParameter() { ParameterName = "@Param1" }));
        }
        /// <summary>
        /// Rejectses the duplicate parameter names in enumerable.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterNamesInEnumerable()
        {
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param1" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            Assert.Throws<ArgumentException>(() => _builder.AddParameterRange(parameters));
        }
        /// <summary>
        /// Rejectses the duplicate parameter names.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterNames()
        {
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "@Param1" };
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameter(parameter);

            Assert.Throws<ArgumentException>(() => _builder.AddParameterRange(parameters));
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
                new CustomDbParameter() { ParameterName = "Param1" },
                new CustomDbParameter() { ParameterName = "Param2" },
                new CustomDbParameter() { ParameterName = "Param3" }
            };

            _builder.AddParameterRange(parameters);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 3);

            _builder.ClearParameters();

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 0);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanFindParameterByName()
        {
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "@Param1", Value = 1};

            _builder.AddParameter(parameter);

            DbParameter param = _builder.GetParameter(parameter.ParameterName);

            Assert.IsNotNull(param);
            Assert.AreEqual(typeof(CustomDbParameter), param.GetType());
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
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "@Param1", Value = 1 };

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
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "@Param1", Value = 1 };
            CustomDbParameter newParam = new CustomDbParameter() { ParameterName="@Param2", Value = "SomeValue" };
            
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
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "@Param1", Value = 1 };

            _builder.AddParameter(parameter);
            _builder.SetParamaterValue(parameter.ParameterName, 333);
            DbParameter param = _builder.GetParameter(parameter.ParameterName);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.That(param.ParameterName == parameter.ParameterName);
            Assert.That(333 == (int)param.Value);
        }
        /// <summary>
        /// Determines whether this instance [can build SQL query].
        /// </summary>
        [Test]
        [Category("QueryBuild")]
        public void BuildSQLQuery()
        {
            CommandType type = _faker.PickRandom<CommandType>();
            string sql = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool clearContents = _faker.Random.Bool();
            bool prepareQuery = _faker.Random.Bool();
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            ISqlQuery query = _builder.CreateSQLQuery(sql, type, commandTimeout, prepareQuery, clearContents);

            Assert.IsNotNull(query);
            Assert.AreEqual(type, query.QueryType);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(sql, query.QueryText);
            Assert.AreEqual(prepareQuery, query.ShouldBePrepared);
            Assert.AreEqual(type, query.QueryType);
            Assert.IsTrue(_builder.Parameters.Count() == ((clearContents == false) ? parameters.Count : 0));
        }
        /// <summary>
        /// Determines whether this instance [can build SQL query].
        /// </summary>
        [Test]
        [Category("QueryBuild")]
        public void BuildSQLQueryNoParameters()
        {
            CommandType type = _faker.PickRandom<CommandType>();
            string sql = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool clearContents = _faker.Random.Bool();
            bool prepareQuery = _faker.Random.Bool();

            ISqlQuery query = _builder.CreateSQLQuery(sql, type, commandTimeout, prepareQuery, clearContents);

            Assert.IsNotNull(query);
            Assert.AreEqual(type, query.QueryType);
            Assert.IsTrue(query.Parameters.Count() == 0);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(sql, query.QueryText);
            Assert.AreEqual(prepareQuery, query.ShouldBePrepared);
            Assert.AreEqual(type, query.QueryType);
        }
        #endregion
    }
}