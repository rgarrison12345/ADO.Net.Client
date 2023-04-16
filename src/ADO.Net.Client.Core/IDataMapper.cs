using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract class that defines the methods that map data from a record in a database to a .NET object
    /// </summary>
    public interface IDataMapper
    {
        /// <summary>
        /// <c>true</c> if columns named like User_ID should be mapped to property called UserID, <c>false</c> otherwise
        /// </summary>
        public bool MatchUnderscoreNames { get; }
       
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        IAsyncEnumerable<T> MapResultSetStreamAsync<T>(DbDataReader reader, CancellationToken token = default) where T : class;
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
    }
}