using ADO.Net.Client.Core;
using ADO.Net.Client.Tests.Common;
using Bogus;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    ///     
    /// </summary>
    [TestFixture]
    public class StoredProcedureBuilderTests
    {
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterBuilder> paramBuilder = new Mock<IDbParameterBuilder>();
        private StoredProcedureQueryBuilder _builder;
        
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            paramBuilder = new Mock<IDbParameterBuilder>();
            _builder = new StoredProcedureQueryBuilder(paramBuilder.Object);
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

            _builder.AddParameterRange(parameters);

            ISqlQuery query = _builder.CreateStoredProcedure(queryString, commandTimeout, prepareQuery, clearContents);

            Assert.IsNotNull(query);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(queryString, query.QueryText);
            Assert.AreEqual(prepareQuery, query.ShouldBePrepared);
            Assert.AreEqual(CommandType.StoredProcedure, query.QueryType);
            Assert.IsTrue(_builder.Parameters.Count() == ((clearContents == false) ? parameters.Count : 0));
        }
    }
}