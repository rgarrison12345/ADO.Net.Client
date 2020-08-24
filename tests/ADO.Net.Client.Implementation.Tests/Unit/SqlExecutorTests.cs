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
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public partial class SqlExecutorTests
    {
        #region Fields/Properties
        private ISqlQuery realQuery;
        private Mock<IConnectionManager> _manager;
        private Mock<IDataMapper> _mapper;
        private Mock<IDbObjectFactory> _factory;
        private Mock<CustomDbCommand> _command;
        private SqlExecutor _executor;
        private readonly Faker _faker = new Faker();
        #endregion
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecutorTests"/> class.
        /// </summary>
        public SqlExecutorTests()
        {
        }
        #endregion
        #region Setup/Teardown        
        /// <summary>
        /// Called when [time setup].
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Mock<ISqlQuery> mockQuery = new Mock<ISqlQuery>();

            mockQuery.Setup(x => x.CommandTimeout).Returns(_faker.Random.Int());
            mockQuery.Setup(x => x.QueryText).Returns(_faker.Random.AlphaNumeric(30));
            mockQuery.Setup(x => x.QueryType).Returns(_faker.PickRandomParam(CommandType.StoredProcedure, CommandType.Text, CommandType.TableDirect));
            mockQuery.Setup(x => x.ShouldBePrepared).Returns(_faker.Random.Bool());
            mockQuery.Setup(x => x.Parameters).Returns(GetDbParameters());

            realQuery = mockQuery.Object;
        }
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _factory = new Mock<IDbObjectFactory>();
            _command = new Mock<CustomDbCommand>();
            _manager = new Mock<IConnectionManager>();
            _mapper = new Mock<IDataMapper>();

            _manager.Setup(x => x.Connection).Returns(new CustomDbConnection());
            _manager.Setup(x => x.Transaction).Returns(GetTransaction());

            SetupFactory();
            
            _executor = new SqlExecutor(_factory.Object, _manager.Object, _mapper.Object);
        }
        #endregion
        #region Helper Methods        
        /// <summary>
        /// Setups the factory.
        /// </summary>
        private void SetupFactory()
        {
            _factory.Setup(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, realQuery.Parameters, _manager.Object.Connection, realQuery.CommandTimeout, _manager.Object.Transaction)).Returns(_command.Object).Verifiable();
        }
        private DbTransaction GetTransaction()
        {
            return (_faker.Random.Bool() == true) ? new CustomDbTransaction() : null;
        }
        /// <summary>
        /// Gets the database parameters.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DbParameter> GetDbParameters()
        {
            //Check if we should generate parameters
            if (_faker.Random.Bool() == true)
            {
                List<CustomDbParameter> parameters = new List<CustomDbParameter>();
                int number = _faker.Random.Int(1, 10);

                //Loop through all numbers
                for (int i = 0; i < number; i++)
                {
                    CustomDbParameter param = new CustomDbParameter();

                    param.ParameterName = $"Parameter{i}";
                    param.Value = _faker.Random.AlphaNumeric(20);

                    parameters.Add(param);
                }

                return parameters;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}