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
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract class that defines the methods that map data from a record in a database to a .NET object
    /// </summary>
    public interface IDataMapper
    {
        #region Utility Methods
#if !NET45
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        IAsyncEnumerable<T> MapResultSetStreamAsync<T>(DbDataReader reader, CancellationToken token = default) where T : class;
#endif
        /// <summary>
        /// Maps an entire result set in the <paramref name="reader"/>
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        Task<IEnumerable<T>> MapResultSetAsync<T>(DbDataReader reader, CancellationToken token = default) where T : class;
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        IEnumerable<T> MapResultSetStream<T>(DbDataReader reader) where T : class;
        /// <summary>
        /// Maps an entire result set in the <paramref name="reader"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        IEnumerable<T> MapResultSet<T>(DbDataReader reader) where T : class;
        /// <summary>
        /// Maps the passed in <paramref name="record"/> to an instance of the <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="record"/></typeparam>
        /// <param name="record">An instance of <see cref="IDataRecord"/> to read data from</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="record"/></returns>
        T MapRecord<T>(IDataRecord record) where T : class;
        #endregion
    }
}