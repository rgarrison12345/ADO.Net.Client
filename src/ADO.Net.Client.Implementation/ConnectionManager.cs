using ADO.Net.Client.Core;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Class that manages a database connection
    /// </summary>
    /// <seealso cref="IConnectionManager" />
    public class ConnectionManager : IConnectionManager
    {
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
                          
        /// <summary>
        /// Replaces the connection.
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> to replace the existing <see cref="Connection"/> with</param>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is <see cref="ConnectionState.Open"/></exception>
        public virtual void ReplaceConnection(DbConnection connection)
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
        public virtual void StartTransaction()
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
        public virtual void StartTransaction(IsolationLevel level)
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
        public virtual void ClearTransaction()
        {
            Transaction = null;
        }
#if !NET462 && !NETSTANDARD2_0
        /// <summary>
        /// Starts a database transaction asynchronously with the specified <paramref name="level"/>
        /// </summary>
        /// <param name="level">Specifies the transaction locking behavior for the <see cref="Connection"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <exception cref="InvalidOperationException">Thrown when the existing <see cref="Connection"/> is not <see cref="ConnectionState.Open"/></exception>
        public virtual async Task StartTransactionAsync(IsolationLevel level, CancellationToken token = default)
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
        public virtual async Task StartTransactionAsync(CancellationToken token = default)
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException($"{nameof(Connection)} is not {nameof(ConnectionState.Open)}");
            }

            Transaction = await Connection.BeginTransactionAsync(token).ConfigureAwait(false);
        }
#endif
    }
}