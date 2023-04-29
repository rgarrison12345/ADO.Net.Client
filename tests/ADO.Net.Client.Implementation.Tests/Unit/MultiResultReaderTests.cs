using ADO.Net.Client.Core;
using ADO.Net.Client.Tests.Common;
using Bogus;
using Moq;
using NUnit.Framework;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public partial class MultiResultReaderTests
    {
        private readonly Faker _faker = new Faker();
        private Mock<IDataMapper> _mockMapper;
        private Mock<CustomDbReader> _mockReader;

        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IDataMapper>();
            _mockReader = new Mock<CustomDbReader>();
        }
    }
}