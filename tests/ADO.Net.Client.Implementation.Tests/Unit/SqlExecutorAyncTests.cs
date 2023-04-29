using Moq;
using NUnit.Framework;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    public partial class SqlExecutorTests
    {      
        /// <summary>
        /// Whens the get database data reader asynchronous is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public async Task WhenGetDbDataReaderAsync_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

#if !NET462
            DbDataReader returned = await _executor.GetDbDataReaderAsync(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior, default);
#else
            DbDataReader returned = await _executor.GetDbDataReaderAsync(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, behavior, default);
#endif

#if !NET462
            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Exactly(1));
            }
            else
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Never);
            }

            _command.Verify(x => x.DisposeAsync(), Times.Exactly(1));
#endif

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, 
                _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, 
                _manager.Object.Transaction), Times.Exactly(1));
        }
        /// <summary>
        /// When the get scalar value async is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public async Task WhenGetScalarValueAsync_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            string expected = _faker.Random.AlphaNumeric(30);

            _command.Setup(x => x.ExecuteScalarAsync(default)).ReturnsAsync(expected);

#if !NET462
            string returned = await _executor.GetScalarValueAsync<string>(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, default);
#else
            string returned = await _executor.GetScalarValueAsync<string>(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, default);
#endif

#if !NET462
            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Exactly(1));
            }
            else
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Never);
            }

            _command.Verify(x => x.DisposeAsync(), Times.Exactly(1));
#endif

            Assert.AreEqual(expected, returned);

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, 
                _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, 
                _manager.Object.Transaction), Times.Once);
            _command.Verify(x => x.ExecuteScalarAsync(default), Times.Exactly(1));
        }
               
        /// <summary>
        /// Whens the execute non query is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public async Task WhenExecuteNonQueryAsync_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            int expected = _faker.Random.Int();

            _command.Setup(x => x.ExecuteNonQueryAsync(default)).ReturnsAsync(expected);

#if !NET462
            int returned = await _executor.ExecuteNonQueryAsync(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, default);
#else
            int returned = await _executor.ExecuteNonQueryAsync(realQuery.QueryText, 
                realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, default);
#endif

#if !NET462
            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Exactly(1));
            }
            else
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Never);
            }

            _command.Verify(x => x.DisposeAsync(), Times.Once);
#endif

            Assert.AreEqual(expected, returned);

            _command.Verify(x => x.ExecuteNonQueryAsync(default), Times.Exactly(1));
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, 
                _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, 
                _manager.Object.Transaction), Times.Exactly(1));
        }
    }
}