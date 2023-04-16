using ADO.Net.Client.Core;
using ADO.Net.Client.Tests.Common;
using Bogus;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public partial class ClientTests
    { 
        private ISqlQuery realQuery;
        private Mock<ISqlExecutor> _executor;
        private readonly Faker _faker = new Faker();
             
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientTests"/> class.
        /// </summary>
        public ClientTests()
        {
        }
      
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _executor = new Mock<ISqlExecutor>();
            Mock<ISqlQuery> mockQuery = new Mock<ISqlQuery>();
            
            mockQuery.Setup(x => x.ShouldBePrepared).Returns(_faker.Random.Bool());
            mockQuery.Setup(x => x.CommandTimeout).Returns(_faker.Random.Int());
            mockQuery.Setup(x => x.QueryText).Returns(_faker.Random.AlphaNumeric(50));
            mockQuery.Setup(x => x.QueryType).Returns(_faker.PickRandom<CommandType>());
            mockQuery.Setup(x => x.Parameters).Returns(GetDbParameters());

            realQuery = mockQuery.Object;
        }

        /// <summary>
        /// Gets the database parameters.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DbParameter> GetDbParameters()
        {
            //Check if we're going to create parameters
            if (_faker.Random.Bool() == true)
            {
                List<CustomDbParameter> parameters = new List<CustomDbParameter>();
                int number = _faker.Random.Int(1, 10);

                //Loop through all numbers
                for (int i = 0; i < number; i++)
                {
                    CustomDbParameter param = new CustomDbParameter
                    {
                        ParameterName = $"Parameter{i}",
                        Value = _faker.Random.AlphaNumeric(20)
                    };

                    parameters.Add(param);
                }

                return parameters;
            }
            else
            {
                return null;
            }
        }
    }
}