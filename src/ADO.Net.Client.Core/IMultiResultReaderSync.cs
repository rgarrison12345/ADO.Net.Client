using System;
using System.Collections.Generic;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for a reader that performs synchronous read operations against a database
    /// </summary>
    public interface IMultiResultReaderSync : IDisposable
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> based on the <typeparamref name="T"/> streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/></returns>
        IEnumerable<T> ReadObjectsStream<T>() where T : class;
        /// <summary>
        /// Gets an entire <see cref="IEnumerable{T}"/> of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> as an entire collection of <typeparamref name="T"/></returns>
        IEnumerable<T> ReadObjects<T>() where T : class;
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create from the query passed into procedure</typeparam>
        /// <returns>Gets an instance of <typeparamref name="T"/></returns>
        T ReadObject<T>() where T : class;
        /// <summary>
        /// Moves to the next result in the underlying data set
        /// </summary>
        /// <returns>Returns <c>true</c> if there's another result set in the underlying data set <c>false</c> otherwise</returns>
        bool MoveToNextResult();
        /// <summary>
        /// Closes the underlying reader object that reads records from the database synchronously
        /// </summary>
        void Close();
    }
}