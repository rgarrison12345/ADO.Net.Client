using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for a reader that performs asynchronous read operations against a database
    /// </summary>
    public interface IMultiResultReaderAsync
#if !NET462 && !NETSTANDARD2_0
        : IAsyncDisposable
#endif
    {
        /// <summary>
        /// Gets an <see cref="IAsyncEnumerable{T}"/> based on the <typeparamref name="T"/> streamed from the server asynchronously
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IAsyncEnumerable{T}"/></returns>
        IAsyncEnumerable<T> ReadObjectsStreamAsync<T>(CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets an entire <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> asynchronously
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> as an entire collection of <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> ReadObjectsAsync<T>(CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> asynchronously
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/></returns>
        Task<T> ReadObjectAsync<T>(CancellationToken token = default) where T : class;
        /// <summary>
        /// Moves to next result set in the underlying data set asynchronously
        /// </summary>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns <c>true</c> if there's another result set in the data set <c>false</c> otherwise</returns>
        Task<bool> MoveToNextResultAsync(CancellationToken token = default);
#if !NET462 && !NETSTANDARD2_0
        /// <summary>
        /// Closes the underlying reader object that reads records from the database asynchronously
        /// </summary>
        Task CloseAsync();
#endif
    }
}