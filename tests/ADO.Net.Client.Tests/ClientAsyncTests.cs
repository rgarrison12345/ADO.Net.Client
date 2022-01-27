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
using System;
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
        /// Whens the get multi result readers asynchronous is cancelled it should throw operation cancelled.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenGetMultiResultReadersAsync_IsCancelled__ItShouldThrowOperationCancelledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).GetMultiResultReaderAsync(realQuery, source.Token));
            }
        }
        /// <summary>
        /// Whens the get multi result readers asynchronous is called it should call SQL executor get multi result reader asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetMultiResultReadersAsync_IsCalled__ItShouldCallSqlExecutorGetMultiResultReaderAsync()
        {
            List<Employee> returnList = new List<Employee>();
            MultiResultReader reader = new MultiResultReader(new CustomDbReader());

#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None)).ReturnsAsync(reader).Verifiable();
#else
            _executor.Setup(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None)).ReturnsAsync(reader).Verifiable();
#endif

            //Make the call
            IMultiResultReader returnedValue = await new DbClient(_executor.Object).GetMultiResultReaderAsync(realQuery, CancellationToken.None);

            Assert.IsNotNull(returnedValue);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None), Times.Once);
#else 
            //Verify the executor was called
            _executor.Verify(x => x.GetMultiResultReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data objects asynchronous is called it should throw operation cancelled exception.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenGetDataObjectsAsync_IsCalled__ItShouldThrowOperationCancelledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).GetDataObjectsAsync<Employee>(realQuery, source.Token));
            }
        }
        /// <summary>
        /// Whens the get data objects asynchronous is called it should call SQL executor get data objects asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetDataObjectsAsync_IsCalled__ItShouldCallSqlExecutorGetDataObjectsAsync()
        {
            List<Employee> returnList = new List<Employee>();

#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None)).ReturnsAsync(returnList).Verifiable();
#else
            _executor.Setup(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None)).ReturnsAsync(returnList).Verifiable();
#endif

            //Make the call
            IEnumerable<Employee> returnedValue = await new DbClient(_executor.Object).GetDataObjectsAsync<Employee>(realQuery, CancellationToken.None);

            Assert.IsNotNull(returnedValue);
            Assert.IsInstanceOf(typeof(List<Employee>), returnedValue);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None), Times.Once);
#else 
            //Verify the executor was called
            _executor.Verify(x => x.GetDataObjectsAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data object asynchronous is called it should throw operation cancelled.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenGetDataObjectAsync_IsCalled__ItShouldThrowOperationCancelled()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).GetDataObjectAsync<Employee>(realQuery, source.Token));
            }
        }
        /// <summary>
        /// Whens the get data object asynchronous is called it should call SQL executor get data object asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetDataObjectAsync_IsCalled__ItShouldCallSqlExecutorGetDataObjectAsync()
        {
#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None)).ReturnsAsync(new Employee()).Verifiable();
#else
            _executor.Setup(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None)).ReturnsAsync(new Employee()).Verifiable();
#endif

            //Make the call
            Employee returnedValue = await new DbClient(_executor.Object).GetDataObjectAsync<Employee>(realQuery, CancellationToken.None);

            Assert.IsNotNull(returnedValue);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None), Times.Once);
#else
            //Verify the executor was called
            _executor.Verify(x => x.GetDataObjectAsync<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get scalar values asynchronous is called it should throw operation canceled exception.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenGetScalarValuesAsync_IsCalled__ItShouldThrowOperationCanceledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).GetScalarValuesAsync<string>(realQuery, source.Token));
            }
        }
        /// <summary>
        /// Whens the get scalar values async is called it should call SQL executor get scalar values async
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetScalarValuesAsync_IsCalled__ItShouldCallSqlExecutorGetScalarValuesAsync()
        {
            List<string> expectedValue = new List<string> { _faker.Random.AlphaNumeric(10), _faker.Random.AlphaNumeric(30), _faker.Random.AlphaNumeric(20) };

#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.GetScalarValuesAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None)).ReturnsAsync(expectedValue).Verifiable();
#else
            _executor.Setup(x => x.GetScalarValuesAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None)).ReturnsAsync(expectedValue).Verifiable();
#endif
            //Make the call
            IEnumerable<string> returnedValue = await new DbClient(_executor.Object).GetScalarValuesAsync<string>(realQuery, CancellationToken.None);

            Assert.AreEqual(expectedValue, returnedValue);
            Assert.IsInstanceOf(typeof(List<string>), returnedValue);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.GetScalarValuesAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None), Times.Once);
#else 
            //Verify the executor was called
            _executor.Verify(x => x.GetScalarValuesAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get scalar value asynchronous is called it should throw operation canceled exception.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenGetScalarValueAsync_IsCalled__ItShouldThrowOperationCanceledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).GetScalarValueAsync<string>(realQuery, source.Token));
            }
        }
        /// <summary>
        /// Whens the get scalar value asynchronous is called it should call SQL executor get scalar value asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetScalarValueAsync_IsCalled__ItShouldCallSqlExecutorGetScalarValueAsync()
        {
            string expectedValue = _faker.Random.AlphaNumeric(30);

#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None)).ReturnsAsync(expectedValue).Verifiable();
#else
            _executor.Setup(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None)).ReturnsAsync(expectedValue).Verifiable();
#endif

            //Make the call
            string returnedValue = await new DbClient(_executor.Object).GetScalarValueAsync<string>(realQuery, CancellationToken.None);

            Assert.IsFalse(string.IsNullOrWhiteSpace(returnedValue));
            Assert.AreEqual(returnedValue, expectedValue);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None), Times.Once);
#else 
            //Verify the executor was called
            _executor.Verify(x => x.GetScalarValueAsync<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get database data reader asynchronous is called it should throw operation canceled exception.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenGetDbDataReaderAsync_IsCalled__ItShouldThrowOperationCanceledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).GetDbDataReaderAsync(realQuery, behavior, source.Token));
            }
        }
        /// <summary>
        /// Whens the get database data reader asynchronous is called it should call SQL executor get database data reader asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public async Task WhenGetDbDataReaderAsync_IsCalled__ItShouldCallSqlExecutorGetDbDataReaderAsync()
        {
            CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior, CancellationToken.None)).ReturnsAsync(new CustomDbReader()).Verifiable();
#else
            _executor.Setup(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, behavior, CancellationToken.None)).ReturnsAsync(new CustomDbReader()).Verifiable();
#endif

            //Make the call
            DbDataReader reader = await new DbClient(_executor.Object).GetDbDataReaderAsync(realQuery, behavior, CancellationToken.None);

            Assert.IsNotNull(reader);
            Assert.IsInstanceOf(typeof(CustomDbReader), reader);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior, CancellationToken.None), Times.Once);
#else 
            //Verify the executor was called
            _executor.Verify(x => x.GetDbDataReaderAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, behavior, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        #endregion
        #region Write Test Methods        
        /// <summary>
        /// Whens the execute non query asynchronous is called it should throw operation canceled.
        /// </summary>
        [Test]
        [Category("Asynchronous Write Tests")]
        public void WhenExecuteNonQueryAsync_IsCalled__ItShouldThrowOperationCanceled()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new DbClient(_executor.Object).ExecuteNonQueryAsync(realQuery, source.Token));
            }
        }
        /// <summary>
        /// Whens the execute non query async is called it should call SQL executor execute non query asynchronous.
        /// </summary>
        [Test]
        [Category("Asynchronous Write Tests")]
        public async Task WhenExecuteNonQueryAsync_IsCalled__ItShouldCallSqlExecutorExecuteNonQueryAsync()
        {
            int returnNumber = _faker.Random.Int(1, 30);

#if !NET461 && !NETCOREAPP2_1
            _executor.Setup(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None)).ReturnsAsync(returnNumber).Verifiable();
#else
            _executor.Setup(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None)).ReturnsAsync(returnNumber).Verifiable();
#endif

            //Make the call
            int records = await new DbClient(_executor.Object).ExecuteNonQueryAsync(realQuery, CancellationToken.None);

            Assert.AreEqual(records, returnNumber);

#if !NET461 && !NETCOREAPP2_1
            //Verify the executor was called
            _executor.Verify(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, CancellationToken.None), Times.Once);
#else 
            //Verify the executor was called
            _executor.Verify(x => x.ExecuteNonQueryAsync(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, CancellationToken.None), Times.Once);
#endif

            _executor.VerifyNoOtherCalls();
        }
        #endregion
        #region Helper Methods     
        /// <summary>
        /// Gets the employees.
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<Employee> GetEmployeesAsync()
        {
            yield return new Employee();
            yield return new Employee();

            await Task.CompletedTask;
        }
        #endregion
    }
}