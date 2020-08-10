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
using ADO.Net.Client.Implementation;
using ADO.Net.Client.Tests.Common;
using ADO.Net.Client.Tests.Common.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Tests
{
    public partial class ClientTests
    {
        #region Read Test Methods       
        /// <summary>
        /// Whens the get data objects asynchronous is called it should call SQL executor get data objects asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetMultiRestultReadersAsync_IsCalled__ItShouldCallSqlExecutorGetMultiResultReaderAsync()
        {
            int delay = _faker.Random.Int(1, 30);

            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(delay))
            {
                Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
                List<Employee> returnList = new List<Employee>();
                MultiResultReader reader = new MultiResultReader(new CustomDbReader());

#if !NET45 && !NET461 && !NETCOREAPP2_1
                mockExecutor.Setup(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token)).ReturnsAsync(reader).Verifiable();
#else
                mockExecutor.Setup(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token)).ReturnsAsync(reader).Verifiable();
#endif

                //Make the call
                IMultiResultReader returnedValue = await new DbClient(mockExecutor.Object).GetMultiResultReaderAsync(realQuery, source.Token);

                Assert.IsNotNull(returnedValue);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token), Times.Once);
#else
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token), Times.Once);
#endif
            }
        }
        /// <summary>
        /// Whens the get data objects asynchronous is called it should call SQL executor get data objects asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetDataObjectsAsync_IsCalled__ItShouldCallSqlExecutorGetDataObjectsAsync()
        {
            int delay = _faker.Random.Int(1, 30);

            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(delay))
            {
                Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>(); 
                List<Employee> returnList = new List<Employee>();

#if !NET45 && !NET461 && !NETCOREAPP2_1
                mockExecutor.Setup(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token)).ReturnsAsync(returnList).Verifiable();
#else
                mockExecutor.Setup(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token)).ReturnsAsync(returnList).Verifiable();
#endif
               
                //Make the call
                IEnumerable<Employee> returnedValue = await new DbClient(mockExecutor.Object).GetDataObjectsAsync<Employee>(realQuery, source.Token);

                Assert.IsNotNull(returnedValue);
                Assert.IsInstanceOf(typeof(List<Employee>), returnedValue);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token), Times.Once);
#else
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token), Times.Once);
#endif
            }
        }
        /// <summary>
        /// Whens the get data object asynchronous is called it should call SQL executor get data object asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetDataObjectAsync_IsCalled__ItShouldCallSqlExecutorGetDataObjectAsync()
        {
            int delay = _faker.Random.Int(1, 30);

            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(delay))
            {
                Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

#if !NET45 && !NET461 && !NETCOREAPP2_1
                mockExecutor.Setup(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token)).ReturnsAsync(new Employee()).Verifiable();
#else
                mockExecutor.Setup(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token)).ReturnsAsync(new Employee()).Verifiable();
#endif

                //Make the call
                Employee returnedValue = await new DbClient(mockExecutor.Object).GetDataObjectAsync<Employee>(realQuery, source.Token);

                Assert.IsNotNull(returnedValue);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token), Times.Once);
#else
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token), Times.Once);
#endif
            }
        }
        /// <summary>
        /// Whens the get scalar vlue asynchronous is called it should call SQL executor get scalar value asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetScalarValueAsync_IsCalled__ItShouldCallSqlExecutorGetScalarValueAsync()
        {
            int delay = _faker.Random.Int(1, 30);

            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(delay))
            {
                Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
                string expectedValue = _faker.Random.AlphaNumeric(30);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                mockExecutor.Setup(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token)).ReturnsAsync(expectedValue).Verifiable();
#else
                mockExecutor.Setup(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token)).ReturnsAsync(expectedValue).Verifiable();
#endif

                //Make the call
                string returnedValue = await new DbClient(mockExecutor.Object).GetScalarValueAsync<string>(realQuery, source.Token);

                Assert.IsFalse(string.IsNullOrWhiteSpace(returnedValue));
                Assert.IsTrue(returnedValue == expectedValue);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token), Times.Once);
#else
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token), Times.Once);
#endif
            }
        }
        /// <summary>
        /// Whens the get database data reader asynchronous is called it should call SQL executor get database data reader asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetDbDataReaderAsync_IsCalled__ItShouldCallSqlExecutorGetDbDataReaderAsync()
        {
            int delay = _faker.Random.Int(1, 30);

            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(delay))
            {
                Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
                CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

#if !NET45 && !NET461 && !NETCOREAPP2_1
                mockExecutor.Setup(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior, source.Token)).ReturnsAsync(new CustomDbReader()).Verifiable();
#else
                mockExecutor.Setup(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, behavior, source.Token)).ReturnsAsync(new CustomDbReader()).Verifiable();
#endif

                //Make the call
                DbDataReader reader = await new DbClient(mockExecutor.Object).GetDbDataReaderAsync(realQuery, behavior, source.Token);

                Assert.IsNotNull(reader);
                Assert.IsInstanceOf(typeof(CustomDbReader), reader);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior, source.Token), Times.Once);
#else
                //Verify the executor was called
                mockExecutor.Verify(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, behavior, source.Token), Times.Once);
#endif
            }
        }
        #endregion
        #region Write Test Methods
        /// <summary>
        /// Whens the execute non query async is called it should call SQL executor execute non query asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Write Tests")]
        public async Task WhenExecuteNonQueryAsync_IsCalled__ItShouldCallSqlExecutorExecuteNonQueryAsync()
        {
            int delay = _faker.Random.Int(1, 30);
            int returnNumber = _faker.Random.Int(1, 30);

            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(delay))
            {
                Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

#if !NET45 && !NET461 && !NETCOREAPP2_0 && !NETCOREAPP2_1
                mockExecutor.Setup(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token)).ReturnsAsync(returnNumber).Verifiable();
#else
                mockExecutor.Setup(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token)).ReturnsAsync(returnNumber).Verifiable();
#endif

                //Make the call
                int records = await new DbClient(mockExecutor.Object).ExecuteNonQueryAsync(realQuery, source.Token);

                Assert.IsTrue(records == returnNumber);

#if !NET45 && !NET461 && !NETCOREAPP2_1
                //Verify the executor was called
                mockExecutor.Verify(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, source.Token), Times.Once);
#else
                //Verify the executor was called
                mockExecutor.Verify(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, source.Token), Times.Once);
#endif
            }
        }
        #endregion
    }
}