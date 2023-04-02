using ADO.Net.Client.Tests.Common;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Reflection;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseTests" />
    [TestFixture]
    [Category("AssemblyNameTests")]
    public class AssemblyTests
    { 
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CanCreateDbFactoryFromAssembly()
        {
            var assembly = Assembly.Load("ADO.Net.Client.Tests.Common");

            DbProviderFactory factory = DbObjectFactory.GetProviderFactory(assembly);

            Assert.IsNotNull(factory);
            Assert.IsInstanceOf(typeof(CustomDbProviderFactory), factory);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ThrowsArugementExceptionCantFindProvider()
        {
            var assembly = Assembly.Load("ADO.Net.Client.Core");

            Assert.Throws<ArgumentException>(() => DbObjectFactory.GetProviderFactory(assembly));
        }
    }
}