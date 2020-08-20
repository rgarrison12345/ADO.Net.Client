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
using Bogus;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Implementation.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category("ConnectionManagerTests")]
    public class ConnectionManagerTests
    {
        #region Fields/Properties
        private readonly Faker _faker = new Faker();
        #endregion
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManagerTests"/> class.
        /// </summary>
        public ConnectionManagerTests()
        {

        }
        #endregion
        #region Tests                        
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
#if !NET45 && !NET461 && !NETCOREAPP2_1
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
        public async Task ThrowsInvalidOperationTransactionStartAsync()
        {
            ConnectionState state = _faker.PickRandom(ConnectionState.Closed, ConnectionState.Broken, ConnectionState.Connecting, ConnectionState.Executing, ConnectionState.Fetching);
            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(state));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.StartTransactionAsync());
        }
        /// <summary>
        /// Throwses the invalid operation transaction start isolation level asynchronous.
        /// </summary>
        [Test]

        public async Task ThrowsInvalidOperationTransactionStartIsolationLevelAsync()
        {
            ConnectionState state = _faker.PickRandom(ConnectionState.Closed, ConnectionState.Broken, ConnectionState.Connecting, ConnectionState.Executing, ConnectionState.Fetching);
            IsolationLevel level = _faker.PickRandom<IsolationLevel>();

            ConnectionManager manager = new ConnectionManager(new CustomDbConnection(state));

            Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.StartTransactionAsync(level));
        }
#endif
        #endregion
    }
}