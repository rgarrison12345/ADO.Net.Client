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
using System.Collections.Generic;
#if !NET45
using System.Runtime.CompilerServices;
#endif
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Implementation
{
    public partial class MultiResultReader
    {
        #region Utility Methods
        /// <summary>
        /// Gets an entire <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> asynchronously
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> as an entire collection of <typeparamref name="T"/></returns>
        public virtual async Task<IEnumerable<T>> ReadObjectsAsync<T>(CancellationToken token = default) where T : class
        {
            //Keep looping through each object in enumerator
            return await _mapper.MapResultSetAsync<T>(_reader, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> asynchronously
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/></returns>
        public virtual async Task<T> ReadObjectAsync<T>(CancellationToken token = default) where T : class
        {
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
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns <c>true</c> if there's another result set in the data set <c>false</c> otherwise</returns>
        public virtual async Task<bool> MoveToNextResultAsync(CancellationToken token = default)
        {
            //Move to next result set
            return await _reader.NextResultAsync(token).ConfigureAwait(false);
        }
#if !NET45
        /// <summary>
        /// Gets an <see cref="IAsyncEnumerable{T}"/> based on the <typeparamref name="T"/> streamed from the server asynchronously
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IAsyncEnumerable{T}"/></returns>
        public virtual async IAsyncEnumerable<T> ReadObjectsStreamAsync<T>([EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            //Keep looping through each object in enumerator
            await foreach (T type in _mapper.MapResultSetStreamAsync<T>(_reader, token).ConfigureAwait(false))
            {
                //Keep yielding results
                yield return type;
            }

            //Nothing to do here
            yield break;
        }
#endif
#if ADVANCE_ASYNC
        /// <summary>
        /// Closes the underlying reader object that reads records from the database asynchronously
        /// </summary>
        public async Task CloseAsync()
        {
            await _reader.CloseAsync().ConfigureAwait(false);
        }
#endif
        #endregion
#if ADVANCE_ASYNC
        #region IDisposableAsync Support 
        /// <summary>
        /// Disposes the asynchronous.
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            return new ValueTask(CloseAsync());
        }
        #endregion
#endif
    }
}
