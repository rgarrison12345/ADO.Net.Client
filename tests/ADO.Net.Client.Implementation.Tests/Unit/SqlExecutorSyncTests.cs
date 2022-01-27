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
#endregion

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    public partial class SqlExecutorTests
    {
        #region Read Test Methods
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
        #endregion
        #region Write Test Methods                
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
        #endregion
    }
}