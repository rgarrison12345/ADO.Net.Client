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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
#endregion

namespace ADO.Net.Client.Tests
{
    public partial class ClientTests
    {
        #region Read Test Methods               
        /// <summary>
        /// Whens the get data object stream is called it should call SQL executor get data object stream.
        /// </summary>
        [Test]
        public void WhenGetDataObjectsStream_IsCalled_ItShouldCallSqlExecutorGetDataObjectsStream()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
            List<Employee> returnedEmployees = new List<Employee>();
            List<Employee> expectedEmployees = GetEmployees();

            mockExecutor.Setup(x => x.GetDataObjectsStream<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(expectedEmployees);

            //Keep iterating through the reader
            foreach(Employee emp in new DbClient(mockExecutor.Object).GetDataObjectsStream<Employee>(realQuery))
            {
                returnedEmployees.Add(emp);
            }

            Assert.IsTrue(expectedEmployees.Count == returnedEmployees.Count);

            //Verify the executor function was called
            mockExecutor.Verify(x => x.GetDataObjectsStream<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get data set is called it should call SQL exectuor get data set.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetDataSet_IsCalled_ItShouldCallSqlExectuorGetDataSet()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetDataSet(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(Mock.Of<DataSet>()).Verifiable();

            //Make the call
            DataSet set = new DbClient(mockExecutor.Object).GetDataSet(realQuery);

            Assert.IsNotNull(set);

            //Verify the executor function was called
            mockExecutor.Verify(x => x.GetDataSet(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get data table is called it should call SQL exectuor get data table.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetDataTable_IsCalled_ItShouldCallSqlExectuorGetDataTable()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetDataTable(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(Mock.Of<DataTable>()).Verifiable();

            //Make the call
            DataTable table = new DbClient(mockExecutor.Object).GetDataTable(realQuery);

            Assert.IsNotNull(table);

            //Verify the executor function was called
            mockExecutor.Verify(x => x.GetDataTable(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get data objects is called it should call SQL exectuor get data objects.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetDataObjects_IsCalled_ItShouldCallSqlExecutorGetDataObjects()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetDataObjects<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(new List<Employee>() { new Employee() }).Verifiable();

            //Make the call
            IEnumerable<Employee> value = new DbClient(mockExecutor.Object).GetDataObjects<Employee>(realQuery);

            Assert.IsTrue(value.Count() == 1);

            //Verify the executor function was called
            mockExecutor.Verify(x => x.GetDataObjects<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get data object is called it should call SQL exectuor get data object.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]

        public void WhenGetDataObject_IsCalled_ItShouldCallSqlExectuorGetDataObject()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetDataObject<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(string.Empty).Verifiable();

            //Make the call
            string value = new DbClient(mockExecutor.Object).GetDataObject<string>(realQuery);

            //Verify the executor function was called
            mockExecutor.Verify(x => x.GetDataObject<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get reader is called it should call SQL executor get reader.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetReader_IsCalled__ItShouldCallSqlExecutorGetReader()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
            CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetDbDataReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior)).Returns(Mock.Of<CustomDbReader>()).Verifiable();

            //Make the call
            DbDataReader reader = new DbClient(mockExecutor.Object).GetDbDataReader(realQuery, behavior);

            Assert.IsNotNull(reader);

            //Verify the executor was called
            mockExecutor.Verify(x => x.GetDbDataReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior), Times.Once);
        }
        /// <summary>
        /// Whens the get scalar values is called it should call SQL executor get scalar values
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetScalarValues_IsCalled__ItShouldCallSqlExecutorGetScalarValues()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
            string[] expectedValue = new string[] { _faker.Random.AlphaNumeric(10), _faker.Random.AlphaNumeric(30), _faker.Random.AlphaNumeric(20) };
               
            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetScalarValues<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(expectedValue).Verifiable();

            //Make the call
            IEnumerable<string> returnedValue = new DbClient(mockExecutor.Object).GetScalarValues<string>(realQuery);

            Assert.AreEqual(expectedValue, returnedValue);

            //Verify the executor was called
            mockExecutor.Verify(x => x.GetScalarValues<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get scalar is called it should call SQL executor get scalar.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetScalar_IsCalled__ItShouldCallSqlExecutorGetScalar()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetScalarValue<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(string.Empty).Verifiable();

            //Make the call
            string value = new DbClient(mockExecutor.Object).GetScalarValue<string>(realQuery);

            //Verify the executor was called
            mockExecutor.Verify(x => x.GetScalarValue<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        /// <summary>
        /// Whens the get multi result reader is called it should call SQL executor get multi result reader.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetMultiResultReader_IsCalled__ItShouldCallSqlExecutorGetMultiResultReader()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();

            //Need to setup the reader function
            mockExecutor.Setup(x => x.GetMultiResultReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(new MultiResultReader(Mock.Of<CustomDbReader>(), Mock.Of<IDataMapper>())).Verifiable();

            //Make the call
            IMultiResultReader reader = new DbClient(mockExecutor.Object).GetMultiResultReader(realQuery);

            Assert.IsNotNull(reader);

            //Verify the executor was called
            mockExecutor.Verify(x => x.GetMultiResultReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        #endregion
        #region Write Test Methods        
        /// <summary>
        /// Whens the execute non query is called it should call SQL executor execute non query.
        /// </summary>
        [Test]
        [Category("Synchronous Write Tests")]
        public void WhenExecuteNonQuery_IsCalled__ItShouldCallSqlExecutorExecuteNonQuery()
        {
            Mock<ISqlExecutor> mockExecutor = new Mock<ISqlExecutor>();
 
            //Need to setup the reader function
            mockExecutor.Setup(x => x.ExecuteNonQuery(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(1).Verifiable();

            //Make the call
            int records = new DbClient(mockExecutor.Object).ExecuteNonQuery(realQuery);

            //Verify the executor was called
            mockExecutor.Verify(x => x.ExecuteNonQuery(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
        }
        #endregion
        #region Helper Methods        
        /// <summary>
        /// Gets the employees.
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployees()
        {
            List<Employee> returnList = new List<Employee>();
            int number = _faker.Random.Int(0, 10);

            for (int i = 0; i < number; i++)
            {
                Employee emp = new Employee()
                {
                    Salary = _faker.Random.Decimal(),
                    PhoneNumber = _faker.Phone.PhoneNumber(),
                    EmployeeID = Guid.NewGuid()
                };

                returnList.Add(emp);
            }

            return returnList;
        }
        #endregion
    }
}