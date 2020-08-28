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
#endregion

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