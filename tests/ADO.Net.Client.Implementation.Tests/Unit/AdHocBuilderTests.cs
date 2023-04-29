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
    [Category("AdHocBuilderTests")]
    public class AdHocBuilderTests
    {
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterBuilder> paramBuilder = new Mock<IDbParameterBuilder>();
        private AdHocQueryBuilder _builder;

        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            paramBuilder = new Mock<IDbParameterBuilder>();
            _builder = new AdHocQueryBuilder(paramBuilder.Object);
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
    }
}