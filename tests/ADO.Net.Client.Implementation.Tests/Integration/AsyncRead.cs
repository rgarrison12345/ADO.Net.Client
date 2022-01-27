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
using ADO.Net.Client.Tests.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Implementation.Tests.Integration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ReadTests
    {
        #region Tests
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
        #endregion
    }
}
