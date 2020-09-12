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
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class QueryFactoryTests
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        #endregion
        #region Tests
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
        #endregion
    }
}