using Moq;
using NUnit.Framework;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    public partial class SqlExecutorTests
    {
        /// <summary>
        /// When the get db data reader is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public void WhenGetDbDataReader_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

            //Make the call
            DbDataReader reader = _executor.GetDbDataReader(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, 
                realQuery.ShouldBePrepared, behavior);

            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.Prepare(), Times.Exactly(1));
            }
            else
            {
                _command.Verify(x => x.Prepare(), Times.Never);
            }

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, 
                _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, 
                _manager.Object.Transaction), Times.Exactly(1));
        }
        /// <summary>
        /// When the get scalar value is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public void WhenGetScalarValue_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            string expected = _faker.Random.AlphaNumeric(30);

            _command.Setup(x => x.ExecuteScalar()).Returns(expected);

            //Make the call
            string returned = _executor.GetScalarValue<string>(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, 
                realQuery.ShouldBePrepared);

            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.Prepare(), Times.Exactly(1));
            }
            else
            {
                _command.Verify(x => x.Prepare(), Times.Never);
            }

            Assert.IsTrue(expected == returned);

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, 
                _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, 
                _manager.Object.Transaction), Times.Exactly(1));
            _command.Verify(x => x.ExecuteScalar(), Times.Exactly(1));
        }               
        /// <summary>
        /// Whens the execute non query is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public void WhenExecuteNonQuery_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            int expected = _faker.Random.Int();

            _command.Setup(x => x.ExecuteNonQuery()).Returns(expected);

            //Make the call
            int returned = _executor.ExecuteNonQuery(realQuery.QueryText, realQuery.QueryType, 
                realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared);

            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.Prepare(), Times.Exactly(1));
            }
            else
            {
                _command.Verify(x => x.Prepare(), Times.Never);
            }

            Assert.IsTrue(expected == returned);

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, 
                _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, 
                _manager.Object.Transaction), Times.Exactly(1));
            _command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(1));
        }
    }
}