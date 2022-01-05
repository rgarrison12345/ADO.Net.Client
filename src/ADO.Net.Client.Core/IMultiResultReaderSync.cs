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
using System;
using System.Collections.Generic;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for a reader that performs synchronous read operations against a database
    /// </summary>
    public interface IMultiResultReaderSync : IDisposable
    {
        #region Utility Methods
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
        #endregion
    }
}