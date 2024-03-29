﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract that defines asynchronous operations to be performed against a data store
    /// </summary>
    public interface IAsynchronousClient
    { 
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streame from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of type parameter object based on the fields in the passed in query</returns>
        IAsyncEnumerable<T> GetDataObjectsStreamAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets an instance of <see cref="IAsyncEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IAsyncEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IAsyncEnumerable<T> GetScalarValuesStreamAsync<T>(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="Task{TResult}"/> of datatable</returns>
        Task<DataTable> GetDataTableAsync(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        Task<T> GetDataObjectAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> of type parameter object based on the fields in the passed in query</returns>
        Task<IEnumerable<T>> GetDataObjectsAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        Task<DbDataReader> GetDbDataReaderAsync(ISqlQuery query, CommandBehavior behavior = CommandBehavior.Default, CancellationToken token = default);
        /// <summary>
        /// Utility method for returning a <see cref="Task{T}"/> value from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task{T}"/></returns>
        Task<T> GetScalarValueAsync<T>(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Utility method for returning a <see cref="Task{IEnumerable}"/> value from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task{T}"/></returns>
        Task<IEnumerable<T>> GetScalarValuesAsync<T>(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Gets an instance of <see cref="IMultiResultReader"/> asynchronously
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IMultiResultReader"/></returns>
        Task<IMultiResultReader> GetMultiResultReaderAsync(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">An instance of <see cref="ISqlQuery"/> used to query a data store</param>
        /// <returns>Returns the number of rows affected by the passed in <paramref name="query"/></returns>
        Task<int> ExecuteNonQueryAsync(ISqlQuery query, CancellationToken token = default);
    }
}