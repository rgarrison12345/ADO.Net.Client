using System.Collections.Generic;

namespace ADO.Net.Client.Implementation
{
    public partial class MultiResultReader
    {    
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> based on the <typeparamref name="T"/> streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/></returns>
        public virtual IEnumerable<T> ReadObjectsStream<T>() where T : class
        {
            //Keep looping through each object in enumerator
            foreach (T type in _mapper.MapResultSetStream<T>(_reader))
            {
                //Keep yielding results
                yield return type;
            }
        }
        /// <summary>
        /// Gets an entire <see cref="IEnumerable{T}"/> of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> as an entire collection of <typeparamref name="T"/></returns>
        public virtual IEnumerable<T> ReadObjects<T>() where T : class
        {
            return _mapper.MapResultSet<T>(_reader);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Gets an instance of <typeparamref name="T"/></returns>
        public virtual T ReadObject<T>() where T : class
        {
            //Move to the next record if possible
            if(!_reader.Read())
            {
                return default;
            }

            return _mapper.MapRecord<T>(_reader);
        }
        /// <summary>
        /// Moves to the next result in the underlying data set
        /// </summary>
        /// <returns>Returns <c>true</c> if there's another result set in the underlying data set <c>false</c> otherwise</returns>
        public virtual bool MoveToNextResult()
        {
            //Move to next result set
            return _reader.NextResult();
        }
        /// <summary>
        /// Closes the underlying reader object that reads records from the database synchronously
        /// </summary>
        public virtual void Close()
        {
            _reader.Close();
        }    
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
    }
}
