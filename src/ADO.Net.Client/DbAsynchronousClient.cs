using ADO.Net.Client.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client
{
    public partial class DbClient
    {
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <returns>Returns a <see cref="Task{T}"/> of <see cref="DataTable"/></returns>
        public override async Task<DataTable> GetDataTableAsync(ISqlQuery query, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            DataTable dt = new DataTable();

            dt.Load(await GetDbDataReaderAsync(query, CommandBehavior.SingleResult, token).ConfigureAwait(false));

            //Return this back to the caller
            return dt;
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public override async Task<T> GetDataObjectAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            //Return this back to the caller
            return await _executor.GetDataObjectAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false);
#else
            //Return this back to the caller
            return await _executor.GetDataObjectAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns an instance of <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override async Task<IEnumerable<T>> GetDataObjectsAsync<T>(ISqlQuery query, CancellationToken token = default) where T : class
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            //Return this back to the caller
            return await _executor.GetDataObjectsAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false);
#else
            //Return this back to the caller
            return await _executor.GetDataObjectsAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public override async Task<DbDataReader> GetDbDataReaderAsync(ISqlQuery query, CommandBehavior behavior = CommandBehavior.Default, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            //Return this back to the caller
            return await _executor.GetDbDataReaderAsync(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, behavior, token).ConfigureAwait(false);
#else
            //Return this back to the caller
            return await _executor.GetDbDataReaderAsync(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, behavior, token).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{T}"/> value from the database
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task"/></returns>
        public override async Task<T> GetScalarValueAsync<T>(ISqlQuery query, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            //Return this back to the caller
            return await _executor.GetScalarValueAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false);
#else
            //Return this back to the caller
            return await _executor.GetScalarValueAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Gets an instance of <see cref="IMultiResultReader" />
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>
        /// Returns an instance of <see cref="IMultiResultReader" />
        /// </returns>
        public override async Task<IMultiResultReader> GetMultiResultReaderAsync(ISqlQuery query, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            return await _executor.GetMultiResultReaderAsync(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false);
#else
            return await _executor.GetMultiResultReaderAsync(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of scalar values
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override async Task<IEnumerable<T>> GetScalarValuesAsync<T>(ISqlQuery query, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            return await _executor.GetScalarValuesAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false);
#else
            return await _executor.GetScalarValuesAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// Gets an instance of <see cref="IAsyncEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IAsyncEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override async IAsyncEnumerable<T> GetDataObjectsStreamAsync<T>(ISqlQuery query, [EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            //Return this back to the caller
            await foreach (T type in _executor.GetDataObjectsStreamAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false))
            {
                yield return type;
            }
#else
            //Return this back to the caller
            await foreach (T type in _executor.GetDataObjectsStreamAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false))
            {
                yield return type;
            }
#endif
        }
        /// <summary>
        /// Gets an instance of <see cref="IAsyncEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IAsyncEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override async IAsyncEnumerable<T> GetScalarValuesStreamAsync<T>(ISqlQuery query, [EnumeratorCancellation] CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            //Return this back to the caller
            await foreach (T type in _executor.GetScalarValuesStreamAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false))
            {
                yield return type;
            }
#else
            //Return this back to the caller
            await foreach (T type in _executor.GetScalarValuesStreamAsync<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false))
            {
                yield return type;
            }
#endif
        }      
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="query">An instance of <see cref="ISqlQuery"/> used to query a data store</param>
        /// <returns>Returns the number of rows affected by the passed in <paramref name="query"/></returns>
        public override async Task<int> ExecuteNonQueryAsync(ISqlQuery query, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if !NET462 && !NETSTANDARD2_0
            return await _executor.ExecuteNonQueryAsync(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, token).ConfigureAwait(false);
#else
            return await _executor.ExecuteNonQueryAsync(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, token).ConfigureAwait(false);
#endif
        }
    }
}