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
using ADO.Net.Client.Tests.Common;
using Bogus;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#endregion

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category("AdHocBuilderTests")]
    public class AdHocBuilderTests
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterBuilder> paramBuilder = new Mock<IDbParameterBuilder>();
        private AdHocQueryBuilder _builder;
        #endregion
        #region Setup/Teardown
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            paramBuilder = new Mock<IDbParameterBuilder>();
            _builder = new AdHocQueryBuilder(paramBuilder.Object);
        }
        #endregion
        #region Tests
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

            Assert.IsTrue(string.IsNullOrWhiteSpace(_builder.QueryText));
        }
        /// <summary>
        /// Determines whether this instance [can build SQL query].
        /// </summary>
        [Test]
        [Category("QueryBuild")]
        public void CanBuildSQLQuery()
        {
            string queryString = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool clearContents = _faker.Random.Bool();
            bool prepareQuery = _faker.Random.Bool();
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.Append(queryString);
            _builder.AddParameterRange(parameters);

            ISqlQuery query = _builder.CreateAdHocQuery(commandTimeout, prepareQuery, clearContents);

            Assert.IsNotNull(query);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(queryString, query.QueryText);
            Assert.AreEqual(prepareQuery, query.ShouldBePrepared);
            Assert.AreEqual(CommandType.Text, query.QueryType);
            Assert.IsTrue(_builder.Parameters.Count() == ((clearContents == false) ? parameters.Count : 0));
            Assert.IsTrue(_builder.QueryText == ((clearContents == false) ? queryString : string.Empty));
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

            Assert.IsTrue(!string.IsNullOrWhiteSpace(_builder.QueryText));
            Assert.IsTrue(valueToAppend == _builder.QueryText);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanAppendStringAndParameter()
        {
            string valueToAppend = "Value To Append";
            CustomDbParameter parameter = new CustomDbParameter() { ParameterName = "Param1" };

            _builder.Append(valueToAppend, parameter);

            Assert.IsNotNull(_builder.Parameters);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_builder.QueryText));
            Assert.IsTrue(valueToAppend == _builder.QueryText);
            Assert.IsTrue(_builder.Parameters.Count() == 1);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanAppendStringAndParameters()
        {
            string valueToAppend = "Value To Append";
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "Param1" },
                new CustomDbParameter() { ParameterName = "Param2" },
                new CustomDbParameter() { ParameterName = "Param3" }
            };

            _builder.Append(valueToAppend, parameters);

            Assert.IsNotNull(_builder.Parameters);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_builder.QueryText));
            Assert.IsTrue(valueToAppend == _builder.QueryText);
            Assert.IsTrue(_builder.Parameters.Count() == 3);
        }
        #endregion
    }
}