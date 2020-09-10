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
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract class for managing a database connection
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
#if ADVANCE_ASYNC               
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