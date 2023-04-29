using Bogus;
using System;
using ADO.Net.Client.Tests.Common.Models;
using ADO.Net.Client.Tests.Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class UtilitiesTest
    {
        private readonly Faker _faker = new Faker();
        
        [Test]
        public void ThrowsExceptionNotEnumGetEnumValue()
        {
            Assert.Throws<ArgumentException>(() => Utilities.GetEnumValue(1));
        }
        [Test]
        public void GetSByteTypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(SByteEnum.Value), TypeCode.SByte);
        }
        [Test]
        public void GetByteTypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(ByteEnum.Value), TypeCode.Byte);
        }
        [Test]
        public void GetInt16TypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(Int16Enum.Value), TypeCode.Int16);
        }
        [Test]
        public void GetInt32TypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(Int32Enum.Value), TypeCode.Int32);
        }
        [Test]
        public void GetInt64TypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(Int64Enum.Value), TypeCode.Int64);
        }
        [Test]
        public void GetUInt64TypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(UInt64Enum.Value), TypeCode.UInt64);
        }
        [Test]
        public void GetUInt32TypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(UInt32Enum.Value), TypeCode.UInt32);
        }
        [Test]
        public void GetUInt16TypeCode()
        {
            Assert.AreEqual(Utilities.GetEnumTypeCode(UInt16Enum.Value), TypeCode.UInt16);
        }
        [Test]
        public void ListEnumerable()
        {
            List<string> list = new List<string>();

            Assert.IsTrue(list.IsEnumerable());
        }
        [Test]
        public void StringNotEnumerable()
        {
            Assert.IsFalse("s".IsEnumerable());
        }
        [Test]
        public void BinaryArrayNotEnumerable()
        {
            Assert.IsFalse(new byte[1].IsEnumerable());
        }
        /// <summary>
        /// Cannot the name of the get property by db field name
        /// </summary>
        [Test]
        public void CannotGetPropertyByDbFieldName()
        {
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            PropertyInfo property = properties.GetPropertyInfoByDbField("SomePropertyName");

            Assert.IsNull(property);
        }
        /// <summary>
        /// Gets the name of the property by.
        /// </summary>
        [Test]
        public void GetPropertyByDbFieldName()
        {
            Employee model = new Employee();
            PropertyInfo[] properties = typeof(Employee).GetProperties();

            PropertyInfo property = properties.GetPropertyInfoByDbField("employeetitle");

            Assert.IsNotNull(property);
            Assert.AreEqual(property.Name, nameof(model.Title));
        }
        /// <summary>
        /// Gets the property by name normal casing.
        /// </summary>
        [Test]
        public void GetPropertyByDbFieldNameNormalCasing()
        {
            Employee model = new Employee();
            PropertyInfo[] properties = typeof(Employee).GetProperties();

            PropertyInfo property = properties.GetPropertyInfoByDbField("EmployeeTitle");

            Assert.IsNotNull(property);
            Assert.AreEqual(property.Name, nameof(model.Title));
        }
        [Test]
        public void CannotFindPropertyByName()
        {
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            PropertyInfo info = Utilities.FindProperty(properties, _faker.Random.AlphaNumeric(1));

            Assert.IsNull(info);
        }
        [Test]
        public void FindPropertyByName()
        {
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            PropertyInfo info = Utilities.FindProperty(properties, "Password");

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Name, "Password");
        }
        [Test]
        public void FindPropertyByDbFieldName()
        {
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            PropertyInfo info = Utilities.FindProperty(properties, "EmployeeTitle");

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Name, "Title");
        }
        /// <summary>
        /// Gets the name of the property by.
        /// </summary>
        [Test]
        public void GetPropertyByDbFieldNameUnderscores()
        {
            Employee model = new Employee();
            PropertyInfo[] properties = typeof(Employee).GetProperties();

            PropertyInfo property = properties.GetPropertyInfoByDbField("e_m_p_l_o_y_e_e_t_i_t_l_e_", true);

            Assert.IsNotNull(property);
            Assert.AreEqual(property.Name, nameof(model.Title));
        }
        /// <summary>
        /// Gets the property by name normal casing.
        /// </summary>
        [Test]
        public void GetPropertyWithUnderscores()
        {
            Employee model = new Employee();
            PropertyInfo[] properties = typeof(Employee).GetProperties();

            PropertyInfo property = properties.GetProperty("S_a_l_a_r_y", true);

            Assert.IsNotNull(property);
            Assert.AreEqual(property.Name, nameof(model.Salary));
        }
        /// <summary>
        /// Cannot the name of the get property by name
        /// </summary>
        [Test]
        public void CannotGetPropertyByName()
        {
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            PropertyInfo property = properties.GetProperty(_faker.Random.AlphaNumeric(1));

            Assert.IsNull(property);
        }
        /// <summary>
        /// Gets the name of the property by.
        /// </summary>
        [Test]
        public void GetPropertyByName()
        {
            Employee model = new Employee();
            PropertyInfo[] properties = typeof(Employee).GetProperties();

            PropertyInfo property = properties.GetProperty(nameof(model.EmployeeID).ToLower());

            Assert.IsNotNull(property);
            Assert.That(property.Name == nameof(model.EmployeeID));
        }
        /// <summary>
        /// Gets the property by name normal casing.
        /// </summary>
        [Test]
        public void GetPropertyByNameNormalCasing()
        {
            Employee model = new Employee();
            PropertyInfo[] properties = typeof(Employee).GetProperties();

            PropertyInfo property = properties.GetProperty(nameof(model.EmployeeID));

            Assert.IsNotNull(property);
            Assert.That(property.Name == nameof(model.EmployeeID));
        }
    }
}