using ADO.Net.Client.Tests.Common;
using Bogus;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class QueryBuilderTests 
    {
        private readonly Faker _faker = new Faker();
        private Mock<IDbParameterBuilder> paramBuilder = new Mock<IDbParameterBuilder>();
        private IQueryBuilder _builder;
              
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilderTests"/> class.
        /// </summary>
        public QueryBuilderTests()
        {
        }
           
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            paramBuilder = new Mock<IDbParameterBuilder>();
            _builder = new CustomQueryBuilder(paramBuilder.Object);
        }
       
        /// <summary>
        /// Containtses the parameter false.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterFalse()
        {
            var param = new CustomDbParameter() 
            { 
                ParameterName = "Param", 
                Value = _faker.Random.Int(), 
                DbType = DbType.Int32 
            };

            _builder.AddParameter(param);

            Assert.IsFalse(_builder.Contains(new CustomDbParameter()));
        }
        /// <summary>
        /// Determines whether [contains parameter true].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterTrue()
        {
            var param = new CustomDbParameter() 
            { 
                ParameterName = "Param", 
                Value = _faker.Random.Int(), 
                DbType = DbType.Int32 
            };

            _builder.AddParameter(param);

            Assert.IsTrue(_builder.Contains(param));
        }
        /// <summary>
        /// Determines whether [contains parameter name false].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterNameFalse()
        {
            var parameters = new List<DbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            Assert.That(_builder.Contains("@Param4") == false);
        }
        /// <summary>
        /// Determines whether [contains parameter name true].
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void ContainsParameterNameTrue()
        {
            var parameters = new List<DbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameterRange(parameters);

            Assert.That(_builder.Contains("@Param1"));
            Assert.That(_builder.Contains("@Param3"));
            Assert.That(_builder.Contains("@Param2"));
        }
        /// <summary>
        /// Rejectses the name of the duplicate parameter.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterName()
        {
            var parameter = new CustomDbParameter() { ParameterName = "@Param1" };

            _builder.AddParameter(parameter);

            Assert.Throws<ArgumentException>(() => _builder.AddParameter(new CustomDbParameter() { ParameterName = "@Param1" }));
        }
        /// <summary>
        /// Rejectses the duplicate parameter names in enumerable.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterNamesInEnumerable()
        {
            var parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param1" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            Assert.Throws<ArgumentException>(() => _builder.AddParameterRange(parameters));
        }
        /// <summary>
        /// Rejectses the duplicate parameter names.
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void RejectsDuplicateParameterNames()
        {
            var parameter = new CustomDbParameter() { ParameterName = "@Param1" };
            var parameters = new List<CustomDbParameter>()
            {
                new CustomDbParameter() { ParameterName = "@Param3" },
                new CustomDbParameter() { ParameterName = "@Param2" },
                new CustomDbParameter() { ParameterName = "@Param1" }
            };

            _builder.AddParameter(parameter);

            Assert.Throws<ArgumentException>(() => _builder.AddParameterRange(parameters));
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanClearParameters()
        {
            var parameters = new List<DbParameter>()
            {
                new CustomDbParameter() { ParameterName = "Param1" },
                new CustomDbParameter() { ParameterName = "Param2" },
                new CustomDbParameter() { ParameterName = "Param3" }
            };

            _builder.AddParameterRange(parameters);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 3);

            _builder.ClearParameters();

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 0);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanFindParameterByName()
        {
            var parameter = new CustomDbParameter() 
            { 
                ParameterName = "@Param1", 
                Value = _faker.Random.Int()
            };

            _builder.AddParameter(parameter);

            DbParameter param = _builder.GetParameter(parameter.ParameterName);

            Assert.IsNotNull(param);
            Assert.AreEqual(typeof(CustomDbParameter), param.GetType());
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.That(param.ParameterName == parameter.ParameterName);
            Assert.That(param.Value == parameter.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanRemoveParameterByName()
        {
            var parameter = new CustomDbParameter() 
            { 
                ParameterName = "@Param1", 
                Value = _faker.Random.Int() 
            };

            _builder.AddParameter(parameter);

            _builder.RemoveParameter(parameter.ParameterName);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Contains(parameter.ParameterName) == false);
            Assert.That(_builder.Parameters.Count() == 0);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanReplaceParameterByName()
        {
            var parameter = new CustomDbParameter() 
            { 
                ParameterName = "@Param1", 
                Value = _faker.Random.Int() 
            };
            var newParam = new CustomDbParameter()
            {
                ParameterName="@Param2", 
                Value = _faker.Random.Int() 
            };
            
            _builder.AddParameter(parameter);

            _builder.ReplaceParameter(parameter.ParameterName, newParam);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.AreNotEqual(parameter, newParam);
            Assert.AreNotEqual(parameter.Value, newParam.Value);
            Assert.AreNotEqual(parameter.ParameterName, newParam.ParameterName);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Parameters")]
        public void CanSetParameterValueByName()
        {
            int newValue = _faker.Random.Int();
            var parameter = new CustomDbParameter() 
            { 
                ParameterName = "@Param1", 
                Value = _faker.Random.Int() 
            };

            _builder.AddParameter(parameter);
            _builder.SetParameterValue(parameter.ParameterName, newValue);
            DbParameter param = _builder.GetParameter(parameter.ParameterName);

            Assert.IsNotNull(_builder.Parameters);
            Assert.That(_builder.Parameters.Count() == 1);
            Assert.That(param.ParameterName == parameter.ParameterName);
            Assert.That(newValue == (int)param.Value);
        }
    }
}