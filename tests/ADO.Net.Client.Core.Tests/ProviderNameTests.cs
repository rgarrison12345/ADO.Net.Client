using ADO.Net.Client.Tests.Common;
using NUnit.Framework;
using System;
using System.Data.Common;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseTests" />
    [TestFixture]
    [Category("ProviderNameTests")]
    public class ProviderNameTests : BaseTests
    {
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public override void Setup()
        {
            _factory = new DbObjectFactory("ADO.Net.Client.Tests.Common");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanCreateDbFactoryFromProviderName()
        {
            DbProviderFactory factory = DbObjectFactory.GetProviderFactory(_commonNamespace);

            //Needs to be a mysql client factory
            Assert.IsNotNull(factory);
            Assert.IsInstanceOf(typeof(CustomDbProviderFactory), factory);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ThrowsArugementExceptionCantFindProvider()
        {
            Assert.Throws<ArgumentException>(() => DbObjectFactory.GetProviderFactory("ADO.Net.Client.Core"));
        }
    }
}