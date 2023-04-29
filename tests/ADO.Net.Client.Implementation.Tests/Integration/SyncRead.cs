using ADO.Net.Client.Tests.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ADO.Net.Client.Implementation.Tests.Integration
{
    public partial class ReadTests
    {
        /// <summary>
        /// Gets the data object asynchronous.
        /// </summary>
        [Test]
        public void GetDataObjects()
        {
            int expectedRead = _faker.Random.Int(1, 10);
            string sql = $"Select Top {expectedRead} * From Employee";

            _manager.Connection.Open();

            IEnumerable<Employee> employees = new SqlExecutor(_factory, _manager).GetDataObjects<Employee>(sql, CommandType.Text, null);

            
            Assert.IsTrue(expectedRead == employees.Count());
        }
        /// <summary>
        /// Gets the data object asynchronous.
        /// </summary>
        [Test]
        public void GetDataObject()
        {
            string sql = $"Select Top 1 * From Employee";

            _manager.Connection.Open();

            Employee employee = new SqlExecutor(_factory, _manager).GetDataObject<Employee>(sql, CommandType.Text, null);

            Assert.IsNotNull(employee);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetDataObjectsStraemA()
        {
            List<Employee> employees = new List<Employee>();
            int expectedRead = _faker.Random.Int(1, 10);
            string sql = $"Select Top {expectedRead} * From Employee";

            IEnumerable<Employee> emplyoees = new SqlExecutor(_factory, _manager).GetDataObjectsStream<Employee>(sql, CommandType.Text, null);

            //Keep going through the enumerable
            foreach (Employee employee in emplyoees)
            {
                employees.Add(employee);
            }

            Assert.IsTrue(employees.Count == expectedRead);
        }
    }
}