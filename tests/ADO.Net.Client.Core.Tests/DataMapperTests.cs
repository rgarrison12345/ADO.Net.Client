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
using ADO.Net.Client.Tests.Common;
using ADO.Net.Client.Tests.Common.Models;
using Bogus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category("Mapper Tests")]
    public class DataMapperTests
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        private DataMapper _mapper;
        #endregion
        #region Constructors
        #endregion
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _mapper = new DataMapper();
        }
        #endregion
        #region Tests        
        /// <summary>
        /// Maps the record create instance.
        /// </summary>
        [Test]
        public void MapRecordCreateInstance()
        {
            decimal salary = _faker.Random.Decimal();
            PhoneType type = _faker.PickRandom<PhoneType>();
            Guid id = _faker.Random.Guid();
            string title = _faker.Random.AlphaNumeric(30);
            int departmentID = _faker.Random.Int();

            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("EmployeeID", id ),
                new KeyValuePair<string, object>("Title", title),
                new KeyValuePair<string, object>("Salary", salary),
                new KeyValuePair<string, object>("DepartmentID", departmentID),
                new KeyValuePair<string, object>("PhoneType", type)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.AreEqual(salary, model.Salary);
            Assert.AreEqual(title, model.Title);
            Assert.AreEqual(id, model.EmployeeID);
            Assert.AreEqual(departmentID, model.DepartmentID);
            Assert.AreEqual(type, model.PhoneType);
        }
        /// <summary>
        /// Maps the record create instance.
        /// </summary>
        [Test]
        public void MapRecordCreateInstanceNotNull()
        {
            CustomDataRecord record = new CustomDataRecord(new List<KeyValuePair<string, object>>());
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsNotNull(model);
        }
        /// <summary>
        /// Maps the normal property database null.
        /// </summary>
        [Test]
        public void MapNormalPropertyDBNull()
        {
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("PhoneNumber", DBNull.Value)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsNull(model.PhoneNumber);
        }
        /// <summary>
        /// Maps the normal property.
        /// </summary>
        [Test]
        public void MapNormalProperty()
        {
            Guid employeedID = Guid.NewGuid();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("EmployeeID", employeedID)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.AreEqual(employeedID, model.EmployeeID);
        }
        /// <summary>
        /// Maps the normal property.
        /// </summary>
        [Test]
        public void MapEnumProperty()
        {
            EmployeeType type = _faker.PickRandom<EmployeeType>();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("Type", type)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.AreEqual(type, model.Type);
        }
        /// <summary>
        /// Maps the normal property.
        /// </summary>
        [Test]
        public void MapNullValueNullEnumProperty()
        {
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("PhoneType", DBNull.Value)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsNull(model.PhoneType);
        }
        /// <summary>
        /// Maps the normal property.
        /// </summary>
        [Test]
        public void MapValueNullEnumProperty()
        {
            PhoneType type = _faker.PickRandom<PhoneType>();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("PhoneType", type)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.AreEqual(type, model.PhoneType);
        }
        [Test]
        public void MapNullableWithValue()
        {
            Guid managerID = Guid.NewGuid();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("ManagerID", managerID)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.AreEqual(managerID, model.ManagerID);
        }
        [Test]
        public void MapNullableWithNullValue()
        { 
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("ManagerID", DBNull.Value)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsNull(model.ManagerID);
        }
        [Test]
        public void MapDbFieldNull()
        {
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("EmployeeTitle", DBNull.Value)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsTrue(model.Title == "SoftwareDeveloper");
        }
        [Test]
        public void MapDbField()
        {
            decimal earnings = _faker.Random.Decimal();

            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("Earnings", earnings)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsTrue(model.Salary == earnings);
        }
        [Test]
        public void MapNonExistentProperty()
        {
            decimal wage = _faker.Random.Decimal();

            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("Wage", wage)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.IsTrue(model.Salary == 0);
        }
        #endregion
    }
}