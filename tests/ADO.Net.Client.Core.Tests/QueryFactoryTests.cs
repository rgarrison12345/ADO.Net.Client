using ADO.Net.Client.Tests.Common;
using Bogus;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class QueryFactoryTests
    {
        private readonly Faker _faker = new Faker();

        [Test]
        public void CreateQuery()
        {
            string sql = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool shouldBePrepared = _faker.Random.Bool();
            CommandType type = _faker.PickRandom<CommandType>();
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            ISqlQuery query = QueryFactory.CreateSQLQuery(sql, type, parameters, commandTimeout, shouldBePrepared);

            Assert.IsNotNull(query);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(sql, query.QueryText);
            Assert.AreEqual(shouldBePrepared, query.ShouldBePrepared);
            Assert.AreEqual(query.Parameters.Count(), parameters.Count);
            Assert.AreEqual(type, query.QueryType);
        }
        [Test]
        public void CreateAdHocQuery()
        {
            string sql = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool shouldBePrepared = _faker.Random.Bool();
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            ISqlQuery query = QueryFactory.CreateAdHocQuery(sql, parameters, commandTimeout, shouldBePrepared);

            Assert.IsNotNull(query);
            Assert.AreEqual(query.QueryType, CommandType.Text);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(sql, query.QueryText);
            Assert.AreEqual(shouldBePrepared, query.ShouldBePrepared);
            Assert.AreEqual(query.Parameters.Count(), parameters.Count);
        }
        [Test]
        public void CreateStoredProcedure()
        {
            string sql = _faker.Random.AlphaNumeric(30);
            int commandTimeout = _faker.Random.Int();
            bool shouldBePrepared = _faker.Random.Bool();
            List<CustomDbParameter> parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            ISqlQuery query = QueryFactory.CreateStoredProcedureQuery(sql, parameters, commandTimeout, shouldBePrepared);

            Assert.IsNotNull(query);
            Assert.AreEqual(query.QueryType, CommandType.StoredProcedure);
            Assert.AreEqual(commandTimeout, query.CommandTimeout);
            Assert.AreEqual(sql, query.QueryText);
            Assert.AreEqual(shouldBePrepared, query.ShouldBePrepared);
            Assert.AreEqual(query.Parameters.Count(), parameters.Count);
        }
    }
}