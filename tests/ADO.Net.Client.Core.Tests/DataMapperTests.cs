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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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

            Assert.IsTrue(model.Salary == default);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void MapPropertyColumnNameHasUnderscores()
        {
            DataMapper mapper = new DataMapper(true);
            decimal salary = _faker.Random.Decimal();

            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("S_a_l_a_r_y", salary)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = mapper.MapRecord<Employee>(record);

            Assert.IsTrue(model.Salary == salary);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void MapFullEmployeeModel()
        {
            bool active = _faker.Random.Bool();
            int departmentID = _faker.Random.Int();
            string phoneNumber = _faker.Phone.PhoneNumber();
            string userName = _faker.Person.FullName;
            decimal salary = _faker.Random.Decimal();
            Guid managerID = _faker.Random.Guid();
            DateTime recordCreated = _faker.Date.Soon();
            PhoneType phoneType = _faker.PickRandom<PhoneType>();

#if NET6_0_OR_GREATER
            TimeOnly lunchTime = _faker.Date.SoonTimeOnly();
            DateOnly hireDate = _faker.Date.SoonDateOnly();
#endif

            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("Earnings", salary),
                new KeyValuePair<string, object>("ManagerID", managerID),
                new KeyValuePair<string, object>("PhoneType", phoneType),
                new KeyValuePair<string, object>("Active", active),
                new KeyValuePair<string, object>("DepartmentID", departmentID),
                new KeyValuePair<string, object>("PhoneNumber", phoneNumber),
                new KeyValuePair<string, object>("RecordCreated", recordCreated),
                new KeyValuePair<string, object>("UserName", userName),
                new KeyValuePair<string, object>("IgnoreField", _faker.Random.Decimal())
            };

#if NET6_0_OR_GREATER
            kvp.Add(new KeyValuePair<string, object>("HireDate", hireDate));
            kvp.Add(new KeyValuePair<string, object>("LunchTime", lunchTime));
#endif

            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = _mapper.MapRecord<Employee>(record);

            Assert.AreEqual(active, model.Active);
            Assert.AreEqual(salary, model.Salary);
            Assert.AreEqual(recordCreated, model.RecordCreated);
            Assert.AreEqual(departmentID, model.DepartmentID);
            Assert.AreEqual(managerID, model.ManagerID);
            Assert.AreEqual(phoneNumber, model.PhoneNumber);
            Assert.AreEqual(phoneType, model.PhoneType);
            Assert.AreEqual(userName, model.UserName);
            Assert.AreEqual(model.IgnoreField, 0m);

#if NET6_0_OR_GREATER
            Assert.AreEqual(hireDate, model.HireDate);
            Assert.AreEqual(lunchTime, model.LunchTime);
#endif
        }
        #endregion
    }
}