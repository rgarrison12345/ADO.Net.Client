using ADO.Net.Client.Tests.Common;
using NUnit.Framework;

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseTests" />
    [TestFixture]
    [Category("DbProviderFactoryTests")]
    public class DbProviderFactoryTests : BaseTests
    {
        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public override void Setup()
        {
            _factory = new DbObjectFactory(CustomDbProviderFactory.Instance);
        }
    }
}