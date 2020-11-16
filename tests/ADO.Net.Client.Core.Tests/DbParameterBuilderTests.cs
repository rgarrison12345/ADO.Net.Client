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
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category("DbParameterBuilderTests")]
    public class DbParameterBuilderTests
    {
        #region Fields/Properties
        private DbParameterBuilder _builder;
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterFormatter> _formatter;
        #endregion
        #region Setup
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _formatter = new Mock<IDbParameterFormatter>();
            _builder = new DbParameterBuilder(CustomDbProviderFactory.Instance, _formatter.Object);
        }
        #endregion
        #region Tests
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ThrowsArguementExceptionValueType()
        {
            Assert.Throws<ArgumentException>(() => _builder.CreateParameters(_faker.Random.Int()));
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ThrowsArguementExceptionString()
        {
            Assert.Throws<ArgumentException>(() => _builder.CreateParameters(_faker.Random.AlphaNumeric(1)));
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Guid value = _faker.Random.Guid();
                PropertyInfo info = typeof(Employee).GetProperty(nameof(Employee.UserName));
                DbParameter param = _builder.CreateParameter(value, info);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateParameterPropertyInfo()
        {
            Guid value = _faker.Random.Guid();
            PropertyInfo info = typeof(Employee).GetProperty(nameof(Employee.ManagerID));
            DbParameter param = _builder.CreateParameter(value, info);

            Assert.IsNotNull(param);
            Assert.IsInstanceOf(typeof(CustomDbParameter), param);
            _formatter.Verify(x => x.MapDbParameter(It.IsAny<DbParameter>(), value, info), Times.Once);
            _formatter.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateParameterName()
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";

            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();

            DbParameter parameter = _builder.CreateParameter(name);

            Assert.IsNotNull(parameter);
            Assert.AreEqual(parameter.ParameterName, name);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateParameterNameValue()
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            int value = _faker.Random.Int();

            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();

            DbParameter parameter = _builder.CreateParameter(name, value);

            Assert.IsNotNull(parameter);
            Assert.AreEqual(parameter.ParameterName, name);
            Assert.AreEqual(parameter.Value, value);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
            _formatter.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateParameterDbTypeDirection()
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            int value = _faker.Random.Int();
            DbType type = _faker.PickRandom<DbType>();
            ParameterDirection direction = _faker.PickRandom<ParameterDirection>();

            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();

            DbParameter parameter = _builder.CreateParameter(name, value, type, direction);

            Assert.IsNotNull(parameter);
            Assert.AreEqual(parameter.ParameterName, name);
            Assert.AreEqual(parameter.Value, value);
            Assert.AreEqual(parameter.DbType, type);
            Assert.AreEqual(parameter.Direction, direction);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
            _formatter.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        [Test]
        [TestCase(10)]
        [TestCase(null)]
        public void CreateVariableSizeParameter(int? size)
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            string value = _faker.Random.AlphaNumeric(40);
            DbType dbType = _faker.PickRandom<DbType>();
            ParameterDirection direction = _faker.PickRandom<ParameterDirection>();

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _builder.CreateParameter(name, value, dbType, size, direction);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(size ?? 0, parameter.Size);
            Assert.AreEqual(direction, parameter.Direction);
            Assert.AreEqual(dbType, parameter.DbType);
            Assert.AreEqual(name, parameter.ParameterName);
            Assert.AreEqual(value, parameter.Value);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
            _formatter.VerifyNoOtherCalls();
        }
#if !NET45
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="precision"></param>
        [Test]
        [TestCase(null, 10)]
        [TestCase(10, null)]
        public void CreateFixedSizeParameter(byte? scale, byte? precision)
        {
            string name = $"@{_faker.Random.AlphaNumeric(30)}";
            int value = _faker.Random.Int();
            DbType dbType = _faker.PickRandom<DbType>();
            ParameterDirection direction = _faker.PickRandom<ParameterDirection>();

            _formatter.Setup(x => x.MapParameterValue(value)).Returns(value).Verifiable();
            _formatter.Setup(x => x.MapParameterName(name)).Returns(name).Verifiable();
            DbParameter parameter = _builder.CreateParameter(name, value, dbType, scale, precision, direction);

            Assert.IsNotNull(parameter);
            Assert.IsInstanceOf(typeof(CustomDbParameter), parameter);
            Assert.AreEqual(scale ?? 0, parameter.Scale);
            Assert.AreEqual(precision ?? 0, parameter.Precision);
            Assert.AreEqual(direction, parameter.Direction);
            Assert.AreEqual(dbType, parameter.DbType);
            Assert.AreEqual(name, parameter.ParameterName);
            Assert.AreEqual(value, parameter.Value);
            _formatter.Verify(x => x.MapParameterName(name), Times.Once);
            _formatter.Verify(x => x.MapParameterValue(value), Times.Once);
            _formatter.VerifyNoOtherCalls();
        }
#endif
        #endregion
    }
}