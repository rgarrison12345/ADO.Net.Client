using System.Data;
using System.Data.Common;
#if !NET462 && !NETSTANDARD2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for managing a database connection
    /// </summary>
    public interface IConnectionManager
    {
        #region Fields/Properties        
        /// <summary>
        /// Gets the connection that is being used by this instance
        /// </summary>
        /// <value>
        /// An instance of <see cref="DbConnection"/>
        /// </value>
        DbConnection Connection { get; }
        /// <summary>
        /// Gets the the <see cref="DbTransaction"/> that was created by the current <see cref="Connection"/>
        /// </summary>
        /// <value>
        /// An instance of <see cref="DbTransaction"/>
        /// </value>
        DbTransaction Transaction { get; }
        #endregion
        #region Utility Methods        
        /// <summary>
        /// Replaces the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        void ReplaceConnection(DbConnection connection);
        /// <summary>
        /// Starts a database transaction
        /// </summary>
        void StartTransaction();
        /// <summary>
        /// Clears the current <see cref="DbTransaction"/>
        /// </summary>
        void ClearTransaction();
        /// <summary>
        /// Starts a database transaction with the specified <paramref name="level"/>
        /// </summary>
        /// <param name="level">Specifies the transaction locking behavior for the <see cref="Connection"/></param>
        void StartTransaction(IsolationLevel level);
#if !NET462 && !NETSTANDARD2_0           
        /// <summary>
        /// Starts a database transaction asynchronously with the specified <paramref name="level"/>
        /// </summary>
        /// <param name="level">Specifies the transaction locking behavior for the <see cref="Connection"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        Task StartTransactionAsync(IsolationLevel level, CancellationToken token = default);
        /// <summary>
        /// Starts a database transaction asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        Task StartTransactionAsync(CancellationToken token = default);
#endif
        #endregion
    }
}