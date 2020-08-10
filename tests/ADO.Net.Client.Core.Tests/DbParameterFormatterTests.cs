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
using Bogus;
using NUnit.Framework;
using System;
using System.Data;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class DbParameterFormatterTests
    {
        #region Fields/Properties
        private DbParameterFormatter _formatter;
        private readonly Faker _faker;
        #endregion
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="DbParameterFormatterTests"/> class.
        /// </summary>
        public DbParameterFormatterTests()
        {
            _faker = new Faker();
        }
        #endregion
        #region Setup/Teardown        
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _formatter = new DbParameterFormatter(_faker.Random.Bool(), _faker.Random.AlphaNumeric(1));
        }
        #endregion
        #region Tests
        [Test]
        [Category("Constructors")]
        public void MapsNativeGuidParamPrefix()
        {
            bool nativeGuid = _faker.Random.Bool();
            string prefix = _faker.Random.AlphaNumeric(1);

            DbParameterFormatter formatter = new DbParameterFormatter(nativeGuid, prefix);

            Assert.IsTrue(formatter.HasNativeGuidSupport == nativeGuid);
            Assert.IsTrue(formatter.ParameterNamePrefix == prefix);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsObjectCorrectlyy()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Object))) == DbType.Object);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsTimeCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Time))) == DbType.Time);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsStringCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.NormalString))) == DbType.String);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapStringFixedLengthCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.StringFixedLength))) == DbType.StringFixedLength);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsANSIStringCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.AnsiString))) == DbType.AnsiString);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsANSIStringFixedLengthCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.AnsiStrinFixedLength))) == DbType.AnsiStringFixedLength);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsDateTimeCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.NormalDateTime))) == DbType.DateTime);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsDateTimeOffsetCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.DateTimeOffset))) == DbType.DateTimeOffset);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsDateTime2Correctly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.DateTime2))) == DbType.DateTime2);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsBinaryCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.ByteArray))) == DbType.Binary);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsFloatCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Float))) == DbType.Single);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsBoolCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Bool))) == DbType.Boolean);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsByteCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Byte))) == DbType.Byte);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsDecimalCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Decimal))) == DbType.Decimal);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsDoubleCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Double))) == DbType.Double);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsSByteCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.SByte))) == DbType.SByte);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsShortCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Short))) == DbType.Int16);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsIntCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Int))) == DbType.Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsLongCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Long))) == DbType.Int64);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsUShortCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.UShort))) == DbType.UInt16);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsUIntCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.UInt))) == DbType.UInt32);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsULongCorrectly()
        {
            Assert.That(_formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.ULong))) == DbType.UInt64);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsNativeGuidCorrectly()
        {
            DbParameterFormatter formatter = new DbParameterFormatter(true);

            Assert.That(formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Guid))) == DbType.Guid);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("DbType")]
        public void MapsNonNativeGuidCorrectly()
        {
            DbParameterFormatter formatter = new DbParameterFormatter(false);

            Assert.That(formatter.MapDbType(typeof(DbTypeModel).GetProperty(nameof(DbTypeModel.Guid))) == DbType.String);
        }
        /// <summary>
        /// Getses the return value direction.
        /// </summary>
        [Test]
        [Category("MapDirection")]
        public void GetsReturnValueDirection()
        {
            PropertyInfo info = typeof(DirectionModel).GetProperty(nameof(DirectionModel.ReturnValue));

            Assert.That(_formatter.MapParameterDirection(info) == ParameterDirection.ReturnValue);
        }
        /// <summary>
        /// Getses the input direcion.
        /// </summary>
        [Test]
        [Category("MapDirection")]
        public void GetsInputDirecion()
        {
            PropertyInfo info = typeof(DirectionModel).GetProperty(nameof(DirectionModel.Input));

            Assert.That(_formatter.MapParameterDirection(info) == ParameterDirection.Input);
        }
        /// <summary>
        /// Getses the output direction.
        /// </summary>
        [Test]
        [Category("MapDirection")]
        public void GetsOutputDirection()
        {
            PropertyInfo info = typeof(DirectionModel).GetProperty(nameof(DirectionModel.Output));

            Assert.That(_formatter.MapParameterDirection(info) == ParameterDirection.Output);
        }
        /// <summary>
        /// Getses the output direction.
        /// </summary>
        [Test]
        [Category("MapDirection")]
        public void GetsInputOutputDirection()
        {
            PropertyInfo info = typeof(DirectionModel).GetProperty(nameof(DirectionModel.InputOutput));

            Assert.That(_formatter.MapParameterDirection(info) == ParameterDirection.InputOutput);
        }
        [Test]
        [Category("MapDirection")]
        public void GetNoSetDirection()
        {
            PropertyInfo info = typeof(DirectionModel).GetProperty(nameof(DirectionModel.NoDirection));

            Assert.That(_formatter.MapParameterDirection(info) == ParameterDirection.InputOutput);
        }
        [Test]
        [Category("MapValue")]
        public void GetsDbNull()
        {
            Assert.AreEqual(_formatter.MapParameterValue(null, null), DBNull.Value);
        }
        [Test]
        [Category("MapValue")]
        public void GetsNormalValue()
        {
            Employee model = new Employee() { Int32 = _faker.Random.Int() };

            Assert.AreEqual(_formatter.MapParameterValue(model.Int32, model.GetType().GetProperty(nameof(model.Int32))), model.Int32);
        }
        [Test]
        [Category("MapValue")]
        public void GetGuidString()
        {
            DbParameterFormatter formatter = new DbParameterFormatter(false);
            Employee model = new Employee()
            {
                EmployeeID= Guid.NewGuid()
            };

            Assert.AreEqual(formatter.MapParameterValue(model.EmployeeID, model.GetType().GetProperty(nameof(model.EmployeeID))), model.EmployeeID.ToString());
        }
        #endregion
    }
}