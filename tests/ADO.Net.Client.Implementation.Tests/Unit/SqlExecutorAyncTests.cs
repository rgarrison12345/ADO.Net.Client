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
using Moq;
using NUnit.Framework;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    public partial class SqlExecutorTests
    {
        #region Read Test Methods        
        /// <summary>
        /// Whens the get database data reader asynchronous is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public async Task WhenGetDbDataReaderAsync_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

#if ADVANCE_ASYNC
            DbDataReader returned = await _executor.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior, default);
#else
            DbDataReader returned = await _executor.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, behavior, default);
#endif

#if ADVANCE_ASYNC
            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Once);
            }
            else
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Never);
            }

            _command.Verify(x => x.DisposeAsync(), Times.Once);
#endif

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, _manager.Object.Transaction), Times.Once);
        }
        /// <summary>
        /// When the get scalar value async is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public async Task WhenGetScalarValueAsync_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            string expected = _faker.Random.AlphaNumeric(30);

            _command.Setup(x => x.ExecuteScalarAsync(default)).ReturnsAsync(expected);

#if ADVANCE_ASYNC
            string returned = await _executor.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, default);
#else
            string returned = await _executor.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, default);
#endif

#if ADVANCE_ASYNC
            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Once);
            }
            else
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Never);
            }

            _command.Verify(x => x.DisposeAsync(), Times.Once);
#endif

            Assert.AreEqual(expected, returned);

            //Verify the calls were made
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, _manager.Object.Transaction), Times.Once);
            _command.Verify(x => x.ExecuteScalarAsync(default), Times.Once);
            _factory.VerifyNoOtherCalls();
            _command.VerifyNoOtherCalls();
        }
        #endregion
        #region Write Test Methods                
        /// <summary>
        /// Whens the execute non query is called it should calls database object factory get database command.
        /// </summary>
        [Test]
        public async Task WhenExecuteNonQueryAsync_IsCalled__ItShouldCallsDbObjectFactory_GetDbCommand()
        {
            int expected = _faker.Random.Int();

            _command.Setup(x => x.ExecuteNonQueryAsync(default)).ReturnsAsync(expected);

#if ADVANCE_ASYNC
            int returned = await _executor.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, default);
#else
            int returned = await _executor.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, default);
#endif

#if ADVANCE_ASYNC
            if (realQuery.ShouldBePrepared == true)
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Once);
            }
            else
            {
                _command.Verify(x => x.PrepareAsync(default), Times.Never);
            }

            _command.Verify(x => x.DisposeAsync(), Times.Once);
#endif

            Assert.AreEqual(expected, returned);

            _command.Verify(x => x.ExecuteNonQueryAsync(default), Times.Once);
            _command.VerifyNoOtherCalls();
            _factory.Verify(x => x.GetDbCommand(realQuery.QueryType, realQuery.QueryText, _manager.Object.Connection, realQuery.Parameters, realQuery.CommandTimeout, _manager.Object.Transaction), Times.Once);
            _factory.VerifyNoOtherCalls();
        }
        #endregion
    }
}