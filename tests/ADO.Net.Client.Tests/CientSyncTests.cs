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

namespace ADO.Net.Client.Tests
{
    public partial class ClientTests
    {        
        /// <summary>
        /// Whens the get scalar values stream is called it should call SQL executor get scalar values stream.
        /// </summary>
        [Test]
        public void WhenGetScalarValuesStream_IsCalled_ItShouldCallSqlExecutorGetScalarValuesStream()
        {
            List<string> returnedValue = new List<string>();
            List<string> expectedValue = GetStringScalarValues();

            _executor.Setup(x => x.GetScalarValuesStream<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(expectedValue);

            //Keep iterating through the reader
            foreach (string value in new DbClient(_executor.Object).GetScalarValuesStream<string>(realQuery))
            {
                returnedValue.Add(value);
            }

            Assert.IsTrue(expectedValue.Count == returnedValue.Count);

            //Verify the executor function was called
            _executor.Verify(x => x.GetScalarValuesStream<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data object stream is called it should call SQL executor get data object stream.
        /// </summary>
        [Test]
        public void WhenGetDataObjectsStream_IsCalled_ItShouldCallSqlExecutorGetDataObjectsStream()
        {
            List<Employee> returnedEmployees = new List<Employee>();
            List<Employee> expectedEmployees = GetEmployees();

            _executor.Setup(x => x.GetDataObjectsStream<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(expectedEmployees);

            //Keep iterating through the reader
            foreach(Employee emp in new DbClient(_executor.Object).GetDataObjectsStream<Employee>(realQuery))
            {
                returnedEmployees.Add(emp);
            }

            Assert.IsTrue(expectedEmployees.Count == returnedEmployees.Count);

            //Verify the executor function was called
            _executor.Verify(x => x.GetDataObjectsStream<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data set is called it should call SQL exectuor get data set.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetDataSet_IsCalled_ItShouldCallSqlExectuorGetDataSet()
        {
            //Need to setup the reader function
            _executor.Setup(x => x.GetDataSet(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(Mock.Of<DataSet>()).Verifiable();

            //Make the call
            DataSet set = new DbClient(_executor.Object).GetDataSet(realQuery);

            Assert.IsNotNull(set);

            //Verify the executor function was called
            _executor.Verify(x => x.GetDataSet(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data table is called it should call SQL exectuor get data table.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetDataTable_IsCalled_ItShouldCallSqlExectuorGetDataTable()
        {
            //Need to setup the reader function
            _executor.Setup(x => x.GetDataTable(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(Mock.Of<DataTable>()).Verifiable();

            //Make the call
            DataTable table = new DbClient(_executor.Object).GetDataTable(realQuery);

            Assert.IsNotNull(table);

            //Verify the executor function was called
            _executor.Verify(x => x.GetDataTable(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data objects is called it should call SQL exectuor get data objects.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetDataObjects_IsCalled_ItShouldCallSqlExecutorGetDataObjects()
        {
            //Need to setup the reader function
            _executor.Setup(x => x.GetDataObjects<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(new List<Employee>() { new Employee() }).Verifiable();

            //Make the call
            IEnumerable<Employee> value = new DbClient(_executor.Object).GetDataObjects<Employee>(realQuery);

            Assert.IsTrue(value.Count() == 1);

            //Verify the executor function was called
            _executor.Verify(x => x.GetDataObjects<Employee>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get data object is called it should call SQL exectuor get data object.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]

        public void WhenGetDataObject_IsCalled_ItShouldCallSqlExectuorGetDataObject()
        {
            //Need to setup the reader function
            _executor.Setup(x => x.GetDataObject<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(string.Empty).Verifiable();

            //Make the call
            string value = new DbClient(_executor.Object).GetDataObject<string>(realQuery);

            //Verify the executor function was called
            _executor.Verify(x => x.GetDataObject<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get reader is called it should call SQL executor get reader.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetReader_IsCalled__ItShouldCallSqlExecutorGetReader()
        {
            CommandBehavior behavior = _faker.PickRandom<CommandBehavior>();

            //Need to setup the reader function
            _executor.Setup(x => x.GetDbDataReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior)).Returns(Mock.Of<CustomDbReader>()).Verifiable();

            //Make the call
            DbDataReader reader = new DbClient(_executor.Object).GetDbDataReader(realQuery, behavior);

            Assert.IsNotNull(reader);

            //Verify the executor was called
            _executor.Verify(x => x.GetDbDataReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared, behavior), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get scalar values is called it should call SQL executor get scalar values
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetScalarValues_IsCalled__ItShouldCallSqlExecutorGetScalarValues()
        {
            List<string> expectedValue = new List<string> { _faker.Random.AlphaNumeric(10), _faker.Random.AlphaNumeric(30), _faker.Random.AlphaNumeric(20) };
               
            //Need to setup the reader function
            _executor.Setup(x => x.GetScalarValues<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(expectedValue).Verifiable();

            //Make the call
            IEnumerable<string> returnedValue = new DbClient(_executor.Object).GetScalarValues<string>(realQuery);

            Assert.AreEqual(expectedValue, returnedValue);
            Assert.IsInstanceOf(typeof(List<string>), returnedValue);

            //Verify the executor was called
            _executor.Verify(x => x.GetScalarValues<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get scalar is called it should call SQL executor get scalar.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetScalar_IsCalled__ItShouldCallSqlExecutorGetScalar()
        {
            //Need to setup the reader function
            _executor.Setup(x => x.GetScalarValue<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(string.Empty).Verifiable();

            //Make the call
            string value = new DbClient(_executor.Object).GetScalarValue<string>(realQuery);

            //Verify the executor was called
            _executor.Verify(x => x.GetScalarValue<string>(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the get multi result reader is called it should call SQL executor get multi result reader.
        /// </summary>
        [Test]
        [Category("Synchronous Read Tests")]
        public void WhenGetMultiResultReader_IsCalled__ItShouldCallSqlExecutorGetMultiResultReader()
        {
            //Need to setup the reader function
            _executor.Setup(x => x.GetMultiResultReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(new MultiResultReader(Mock.Of<CustomDbReader>(), Mock.Of<IDataMapper>())).Verifiable();

            //Make the call
            IMultiResultReader reader = new DbClient(_executor.Object).GetMultiResultReader(realQuery);

            Assert.IsNotNull(reader);

            //Verify the executor was called
            _executor.Verify(x => x.GetMultiResultReader(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the execute non query is called it should call SQL executor execute non query.
        /// </summary>
        [Test]
        [Category("Synchronous Write Tests")]
        public void WhenExecuteNonQuery_IsCalled__ItShouldCallSqlExecutorExecuteNonQuery()
        {
            int expectedRecords = _faker.Random.Int(1, 10);

            //Need to setup the reader function
            _executor.Setup(x => x.ExecuteNonQuery(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared)).Returns(expectedRecords).Verifiable();

            //Make the call
            int returnedRecords = new DbClient(_executor.Object).ExecuteNonQuery(realQuery);

            Assert.AreEqual(expectedRecords, returnedRecords);

            //Verify the executor was called
            _executor.Verify(x => x.ExecuteNonQuery(realQuery.QueryText, realQuery.QueryType, realQuery.Parameters, realQuery.CommandTimeout, realQuery.ShouldBePrepared), Times.Once);
            _executor.VerifyNoOtherCalls();
        }      
        /// <summary>
        /// .
        /// </summary>
        /// <returns></returns>
        public List<string> GetStringScalarValues()
        {
            List<string> returnList = new List<string>();
            int number = _faker.Random.Int(0, 10);

            for (int i = 0; i < number; i++)
            {
                returnList.Add(_faker.Random.AlphaNumeric(10));
            }

            return returnList;
        }
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
    }
}