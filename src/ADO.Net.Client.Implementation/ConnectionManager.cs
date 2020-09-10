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
using ADO.Net.Client.Core;
using System;
using System.Data;
using System.Data.Common;
#if !NET45 && !NET461 && !NETSTANDARD2_0
using System.Threading;
using System.Threading.Tasks;
#endif
#endregion

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Class that manages a database connection
    /// </summary>
    /// <seealso cref="IConnectionManager" />
    public class ConnectionManager : IConnectionManager
    {
        #region Fields/Properties
        /// <summary>
        /// Gets the connection that is being used by this instance
        /// </summary>
        /// <value>
        /// An instance of <see cref="DbConnection" />
        /// </value>
        public DbConnection Connection { get; private set; }
        /// <summary>
        /// Gets the the <see cref="DbTransaction"/> that was created by the current <see cref="Connection" />
        /// </summary>
        /// <value>
        /// An instance of <see cref="DbTransaction" />
        /// </value>
        public DbTransaction Transaction { get; private set; }
        #endregion
        #region Constructors		        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManager"/> class.
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="transaction">An instance of <see cref="DbTransaction"/></param>
        public ConnectionManager(DbConnection connection, DbTransaction transaction) : this(connection)
        {
            Transaction = transaction;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManager"/> class.
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        public ConnectionManager(DbConnection connection)
        {
            Connection = connection;
        }
        #endregion
        #region Utility Methods                        
        /// <summary>
        /// Replaces the connection.
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> to replace the existing <see cref="Connection"/> with</param>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is <see cref="ConnectionState.Open"/></exception>
        public void ReplaceConnection(DbConnection connection)
        {
            if (Connection.State == ConnectionState.Open)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is open, close the existing {nameof(Connection)} and commit/rollback any existing transactions");
            }

            Connection = connection;
        }
        /// <summary>
        /// Starts a database transaction
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is not <see cref="ConnectionState.Open"/></exception>
        public void StartTransaction()
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is not {nameof(ConnectionState.Open)}");
            }

            Transaction = Connection.BeginTransaction();
        }
        /// <summary>
        /// Starts a database transaction with the specified <paramref name="level"/>
        /// </summary>
        /// <param name="level">Specifies the transaction locking behavior for the <see cref="Connection"/></param>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is not <see cref="ConnectionState.Open"/></exception>
        public void StartTransaction(IsolationLevel level)
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is not {nameof(ConnectionState.Open)}");
            }

            Transaction = Connection.BeginTransaction(level);
        }
        /// <summary>
        /// Clears the current <see cref="DbTransaction"/>
        /// </summary>
        public void ClearTransaction()
        {
            Transaction = null;
        }
#if ADVANCE_ASYNC
        /// <summary>
        /// Starts a database transaction asynchronously with the specified <paramref name="level"/>
        /// </summary>
        /// <param name="level">Specifies the transaction locking behavior for the <see cref="Connection"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is not <see cref="ConnectionState.Open"/></exception>
        public async Task StartTransactionAsync(IsolationLevel level, CancellationToken token = default)
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is not {nameof(ConnectionState.Open)}");
            }

            Transaction = await Connection.BeginTransactionAsync(level, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Starts a database transaction asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is not <see cref="ConnectionState.Open"/></exception>
        public async Task StartTransactionAsync(CancellationToken token = default)
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is not {nameof(ConnectionState.Open)}");
            }

            Transaction = await Connection.BeginTransactionAsync(token).ConfigureAwait(false);
        }
#endif
        #endregion
    }
}