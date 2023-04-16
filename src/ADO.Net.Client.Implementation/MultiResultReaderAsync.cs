using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Implementation
{
    public partial class MultiResultReader
    {
        /// <summary>
        /// Gets an entire <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> asynchronously
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> as an entire collection of <typeparamref name="T"/></returns>
        public virtual async Task<IEnumerable<T>> ReadObjectsAsync<T>(CancellationToken token = default) where T : class
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Keep looping through each object in enumerator
            return await _mapper.MapResultSetAsync<T>(_reader, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> asynchronously
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/></returns>
        public virtual async Task<T> ReadObjectAsync<T>(CancellationToken token = default) where T : class
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Move to the next record if possible
            if (!await _reader.ReadAsync(token).ConfigureAwait(false))
            {
                return default;
            }

            return _mapper.MapRecord<T>(_reader);
        }
        /// <summary>
        /// Moves to next result set in the underlying data set asynchronously
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns <c>true</c> if there's another result set in the data set <c>false</c> otherwise</returns>
        public virtual async Task<bool> MoveToNextResultAsync(CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Move to next result set
            return await _reader.NextResultAsync(token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets an <see cref="IAsyncEnumerable{T}"/> based on the <typeparamref name="T"/> streamed from the server asynchronously
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IAsyncEnumerable{T}"/></returns>
        public virtual async IAsyncEnumerable<T> ReadObjectsStreamAsync<T>([EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Keep looping through each object in enumerator
            await foreach (T type in _mapper.MapResultSetStreamAsync<T>(_reader, token).ConfigureAwait(false))
            {
                //Keep yielding results
                yield return type;
            }
        }
        
#if !NET462 && !NETSTANDARD2_0
        /// <summary>
        /// Closes the underlying reader object that reads records from the database asynchronously
        /// </summary>
        public async Task CloseAsync()
        {
            await _reader.CloseAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Disposes the asynchronous.
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            return new ValueTask(CloseAsync());
        }
#endif
    }
}
