using ADO.Net.Client.Tests.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ADO.Net.Client.Implementation.Tests.Integration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ReadTests
    {
        /// <summary>
        /// Gets the data object asynchronous.
        /// </summary>
        [Test]
        public async Task GetDataObjectAsync()
        {
            string sql = "Select Top 1 * From Employee Order By EmployeeID Asc";

            Employee card = await new SqlExecutor(_factory, _manager).GetDataObjectAsync<Employee>(sql, CommandType.Text, null);

            Assert.IsNotNull(card);
        }
        /// <summary>
        /// Gets the data object asynchronous.
        /// </summary>
        [Test]
        public async Task GetDataObjectsAsync()
        {
            int expectedRead = _faker.Random.Int(1, 10);
            string sql = $"Select Top {expectedRead} * From Employee";

            IEnumerable<Employee> employees = await new SqlExecutor(_factory, _manager).GetDataObjectsAsync<Employee>(sql, CommandType.Text, null);

            Assert.IsTrue(employees.Count() == expectedRead);
        }
        /// <summary>
        /// Gets the data object asynchronous.
        /// </summary>
        [Test]
        public async Task GetDataObjectsStraemAsync()
        {
            List<Employee> employees = new List<Employee>();
            int expectedRead = _faker.Random.Int(1, 10);
            string sql = $"Select Top {expectedRead} * From Employee";

            IAsyncEnumerable<Employee> emplyoees = new SqlExecutor(_factory, _manager).GetDataObjectsStreamAsync<Employee>(sql, CommandType.Text, null);

            //Keep going through the enumerable
            await foreach(Employee employee in emplyoees)
            {
                employees.Add(employee);
            }

            Assert.IsTrue(employees.Count == expectedRead);
        }
    }
}
