﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Base class for all classes that query a database
    /// </summary>
    /// <seealso cref="IDbProvider" />
    public abstract class DbProvider : IDbProvider
    {
        /// <summary>
        /// An instance of <see cref="IConnectionManager"/>
        /// </summary>
        public abstract IConnectionManager ConnectionManager { get; }
            
        /// <summary>
        /// Utility method for returning a <see cref="Task{T}"/> value from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column as <see cref="Task{T}"/></returns>
        public abstract Task<T> GetScalarValueAsync<T>(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Utility method for returning a <see cref="Task{T}"/> value from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task{T}"/></returns>
        public abstract Task<IEnumerable<T>> GetScalarValuesAsync<T>(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Gets an instance of <see cref="IMultiResultReader"/> asynchronously
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IMultiResultReader"/></returns>
        public abstract Task<IMultiResultReader> GetMultiResultReaderAsync(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public abstract Task<DbDataReader> GetDbDataReaderAsync(ISqlQuery query, CommandBehavior behavior = CommandBehavior.Default, CancellationToken token = default);
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="Task{TResult}"/> of datatable</returns>
        public abstract Task<DataTable> GetDataTableAsync(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> of type parameter object based on the fields in the passed in query</returns>
        public abstract Task<IEnumerable<T>> GetDataObjectsAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public abstract Task<T> GetDataObjectAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streame from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of type parameter object based on the fields in the passed in query</returns>
        public abstract IAsyncEnumerable<T> GetDataObjectsStreamAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets an instance of <see cref="IAsyncEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IAsyncEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public abstract IAsyncEnumerable<T> GetScalarValuesStreamAsync<T>(ISqlQuery query, CancellationToken token = default);
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public abstract T GetDataObject<T>(ISqlQuery query) where T : class;
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine, streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public abstract IEnumerable<T> GetDataObjectsStream<T>(ISqlQuery query) where T : class;
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public abstract IEnumerable<T> GetDataObjects<T>(ISqlQuery query) where T : class;
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public abstract DataSet GetDataSet(ISqlQuery query);
        /// <summary>
        /// Utility method for returning a <see cref="DataTable"/> object created from the passed in query
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="DataTable"/> object</returns>
        public abstract DataTable GetDataTable(ISqlQuery query);
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in query
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public abstract DbDataReader GetDbDataReader(ISqlQuery query, CommandBehavior behavior = CommandBehavior.Default);
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as an instance of <typeparamref name="T"/></returns>
        public abstract T GetScalarValue<T>(ISqlQuery query);
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public abstract IEnumerable<T> GetScalarValuesStream<T>(ISqlQuery query);
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of scalar values
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public abstract IEnumerable<T> GetScalarValues<T>(ISqlQuery query);
        /// <summary>
        /// Gets an instance of <see cref="IMultiResultReader"/>
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="IMultiResultReader"/></returns>
        public abstract IMultiResultReader GetMultiResultReader(ISqlQuery query);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure
        /// </summary>
        /// <param name="query">An instance of <see cref="ISqlQuery"/> used to query a data store</param>
        /// <returns>Returns the amount of records affected by the passed in <paramref name="query"/></returns>
        public abstract int ExecuteNonQuery(ISqlQuery query);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">An instance of <see cref="ISqlQuery"/> used to query a data store</param>
        /// <returns>Returns the number of rows affected by the passed in <paramref name="query"/></returns>
        public abstract Task<int> ExecuteNonQueryAsync(ISqlQuery query, CancellationToken token = default);
    }
}