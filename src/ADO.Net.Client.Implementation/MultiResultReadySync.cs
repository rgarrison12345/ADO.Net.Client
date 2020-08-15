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
#endregion

namespace ADO.Net.Client.Implementation
{
    public partial class MultiResultReader
    {
        #region Utility Methods        
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> based on the <typeparamref name="T"/> streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/></returns>
        public IEnumerable<T> ReadObjectsStream<T>() where T : class
        {
            //Keep looping through each object in enumerator
            foreach (T type in _mapper.MapResultSetStream<T>(_reader))
            {
                //Keep yielding results
                yield return type;
            }

            //Nothing to do here
            yield break;
        }
        /// <summary>
        /// Gets an entire <see cref="IEnumerable{T}"/> of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> as an entire collection of <typeparamref name="T"/></returns>
        public IEnumerable<T> ReadObjects<T>() where T : class
        {
            return _mapper.MapResultSet<T>(_reader);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Gets an instance of <typeparamref name="T"/></returns>
        public T ReadObject<T>() where T : class
        {
            //Move to the next record if possible
            if(_reader.Read() == false)
            {
                return default;
            }

            return _mapper.MapRecord<T>(_reader);
        }
        /// <summary>
        /// Moves to the next result in the underlying data set
        /// </summary>
        /// <returns>Returns <c>true</c> if there's another result set in the underlying data set <c>false</c> otherwise</returns>
        public bool MoveToNextResult()
        {
            //Move to next result set
            return _reader.NextResult();
        }
        /// <summary>
        /// Closes the underlying reader object that reads records from the database synchronously
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }
        #endregion
        #region IDisposable Support            
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            //Check if we haven't disposed
            if (!disposedValue)
            {
                //Check if we are disposing of managed objects
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
