using ADO.Net.Client.Tests.Common;
using Bogus;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ADO.Net.Client.Implementation.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category("ConnectionManagerTests")]
    public class ConnectionManagerTests
    {
        private readonly Faker _faker = new Faker();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManagerTests"/> class.
        /// </summary>
        public ConnectionManagerTests()
        {

        }
                     
        /// <summary>
        /// Tests the start transaction.
        /// </summary>
        [Test]
        public void TestStartTransactionIsolationLevel()
        {
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection()); 
            IsolationLevel level = _faker.PickRandom<IsolationLevel>();

            manager.StartTransaction(level);

            Assert.IsNotNull(manager.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), manager.Transaction);
            Assert.AreEqual(manager.Transaction.IsolationLevel, level);
        }
        /// <summary>
        /// Tests the start transaction.
        /// </summary>
        [Test]
        public void TestStartTransaction()
        {
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection());

            manager.StartTransaction();

            Assert.IsNotNull(manager.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), manager.Transaction);
        }
        /// <summary>
        /// Clears the transaction.
        /// </summary>
        [Test]
        public void TestClearTransaction()
        {
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(), new CustomDbTransaction());

            Assert.IsNotNull(manager.Connection);
            Assert.IsNotNull(manager.Transaction);

            manager.ClearTransaction();

            Assert.IsNull(manager.Transaction);
        }
        /// <summary>
        /// Throwses the invalid operation replace.
        /// </summary>
        [Test]
        public void ThrowsInvalidOperationConnectionOpenReplace()
        {
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection());

            Assert.Throws<InvalidOperationException>(() => manager.ReplaceConnection(new CustomDbConnection()));
        }
        /// <summary>
        /// Throwses the invalid operation transaction start.
        /// </summary>
        [Test]
        public void ThrowsInvalidOperationTransactionStart()
        {
            ConnectionState state = _faker.PickRandom(ConnectionState.Closed, ConnectionState.Broken, ConnectionState.Connecting, ConnectionState.Executing, ConnectionState.Fetching);
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(state));

            Assert.Throws<InvalidOperationException>(() => manager.StartTransaction());
        }
        /// <summary>
        /// Throwses the invalid operation transaction start isolation level.
        /// </summary>
        [Test]
        public void ThrowsInvalidOperationTransactionStartIsolationLevel()
        {
            ConnectionState state = _faker.PickRandom(ConnectionState.Closed, ConnectionState.Broken, ConnectionState.Connecting, ConnectionState.Executing, ConnectionState.Fetching);
            IsolationLevel level = _faker.PickRandom<IsolationLevel>();
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(state));

            Assert.Throws<InvalidOperationException>(() => manager.StartTransaction(level));
        }
#if !NET462
        /// <summary>
        /// Throwses the invalid operation transaction start asynx.
        /// </summary>
        [Test]
        public async Task StartTransactionStartIsolationLevelAsync()
        {
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection());
            IsolationLevel level = _faker.PickRandom<IsolationLevel>();

            await manager.StartTransactionAsync(level);

            Assert.IsNotNull(manager.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), manager.Transaction);
            Assert.AreEqual(manager.Transaction.IsolationLevel, level);
        }
        /// <summary>
        /// Throwses the invalid operation transaction start asynx.
        /// </summary>
        [Test]
        public async Task StartTransactionStartAsync()
        {
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection());

            await manager.StartTransactionAsync();

            Assert.IsNotNull(manager.Transaction);
            Assert.IsInstanceOf(typeof(CustomDbTransaction), manager.Transaction);
        }
        /// <summary>
        /// Throwses the invalid operation transaction start asynx.
        /// </summary>
        [Test]
        public void ThrowsInvalidOperationTransactionStartAsync()
        {
            ConnectionState state = _faker.PickRandom(ConnectionState.Closed, ConnectionState.Broken, ConnectionState.Connecting, ConnectionState.Executing, ConnectionState.Fetching);
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(state));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.StartTransactionAsync());
        }
        /// <summary>
        /// Throwses the invalid operation transaction start isolation level asynchronous.
        /// </summary>
        [Test]
        public void ThrowsInvalidOperationTransactionStartIsolationLevelAsync()
        {
            ConnectionState state = _faker.PickRandom(ConnectionState.Closed, ConnectionState.Broken, ConnectionState.Connecting, ConnectionState.Executing, ConnectionState.Fetching);
            IsolationLevel level = _faker.PickRandom<IsolationLevel>();

            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(state));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.StartTransactionAsync(level));
        }
#endif
    }
}