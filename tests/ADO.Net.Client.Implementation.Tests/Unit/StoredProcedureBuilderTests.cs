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
    public class StoredProcedureBuilderTests
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterBuilder> paramBuilder = new Mock<IDbParameterBuilder>();
        private StoredProcedureQueryBuilder _builder;
        #endregion
        #region Setup/Teardown
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            paramBuilder = new Mock<IDbParameterBuilder>();
            _builder = new StoredProcedureQueryBuilder(paramBuilder.Object);
        }
        #endregion
        #region Tests
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

            _builder.AddParameterRange(parameters);

            ISqlQuery query = _builder.CreateStoredProcedure(queryString, commandTimeout, prepareQuery, clearContents);

            Assert.IsNotNull(query);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(queryString, query.QueryText);
            Assert.AreEqual(prepareQuery, query.ShouldBePrepared);
            Assert.AreEqual(CommandType.StoredProcedure, query.QueryType);
            Assert.IsTrue(_builder.Parameters.Count() == ((clearContents == false) ? parameters.Count : 0));
        }
        #endregion
    }
}