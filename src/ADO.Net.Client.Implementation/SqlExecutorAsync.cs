﻿#region Licenses
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
#region Using Declarations
using ADO.Net.Client.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#if !NET45
using System.Runtime.CompilerServices;
#endif
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Implementation
{
    public partial class SqlExecutor
    {
        #region Data Retrieval
        /// <summary>
        /// Utility method for returning an <see cref="IAsyncEnumerable{T}"/> of scalar values streamed from the database
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <typeparam name="T">The data type to return from data value returned from the query</typeparam>
        /// <returns>Returns an <see cref="IAsyncEnumerable{T}"/> of the value of the first column in the result set as an instance of <typeparamref name="T"/></returns>
        public virtual async IAsyncEnumerable<T> GetScalarValuesStreamAsync<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, [EnumeratorCancellation] CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await GetDbDataReaderAsync(query, queryCommandType, parameters, commandTimeout, CommandBehavior.SingleResult, token).ConfigureAwait(false))
            {
                //Keep reading through the results
                while (await reader.ReadAsync(token).ConfigureAwait(false))
                {
                    //Check if we need a default value
                    if (await reader.IsDBNullAsync(0, token).ConfigureAwait(false))
                    {
                        yield return default;
                    }
                    else
                    {
                        yield return await reader.GetFieldValueAsync<T>(0, token).ConfigureAwait(false);
                    }
                }
            }

            yield break;
        }
        /// <summary>
        /// Gets an <see cref="IAsyncEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IAsyncEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public virtual async IAsyncEnumerable<T> GetDataObjectsStreamAsync<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, [EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await GetDbDataReaderAsync(query, queryCommandType, parameters, commandTimeout, CommandBehavior.SingleResult, token).ConfigureAwait(false))
            {
                //Check if the reader has rows first
                if (reader.HasRows)
                {
                    //Keep iterating
                    await foreach (T t in _mapper.MapResultSetStreamAsync<T>(reader, token).ConfigureAwait(false))
                    {
                        //Return this back to the caller
                        yield return t;
                    }
                }

                //Nothing to do here
                yield break;
            }
        }
        /// <summary>
        /// Utility method for returning an <see cref="IEnumerable{T}"/> of scalar values from the database
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <typeparam name="T">The data type to return from data value returned from the query</typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of the value of the first column in the result set as an instance of <typeparamref name="T"/></returns>
        public virtual async Task<IEnumerable<T>> GetScalarValuesAsync<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            List<T> returnList = new List<T>();

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await GetDbDataReaderAsync(query, queryCommandType, parameters, commandTimeout, CommandBehavior.SingleResult, token).ConfigureAwait(false))
            {
                //Keep reading through the results
                while (await reader.ReadAsync(token).ConfigureAwait(false))
                {
                    //Check if we need a default value
                    if (await reader.IsDBNullAsync(0, token).ConfigureAwait(false))
                    {
                        returnList.Add(default);
                    }
                    else
                    {
                        returnList.Add(await reader.GetFieldValueAsync<T>(0, token).ConfigureAwait(false));
                    }
                }
            }

            return returnList;
        }
        /// <summary>
        /// Gets an instance of the <typeparamref name="T"/> parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public virtual async Task<T> GetDataObjectAsync<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CancellationToken token = default) where T : class
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await GetDbDataReaderAsync(query, queryCommandType, parameters, commandTimeout, CommandBehavior.SingleRow, token).ConfigureAwait(false))
            {
                //Check if the reader has rows
                if (reader.HasRows)
                {
                    //Move to the first record in the result set
                    await reader.ReadAsync(token).ConfigureAwait(false);

                    //Return this back to the caller
                    return _mapper.MapRecord<T>(reader);
                }

                return default;
            }
        }
        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public virtual async Task<IEnumerable<T>> GetDataObjectsAsync<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CancellationToken token = default) where T : class
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await GetDbDataReaderAsync(query, queryCommandType, parameters, commandTimeout, CommandBehavior.SingleResult, token).ConfigureAwait(false))
            {
                //Get the field name and value pairs out of this query
                return await _mapper.MapResultSetAsync<T>(reader, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader</returns>
        public virtual async Task<DbDataReader> GetDbDataReaderAsync(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CommandBehavior behavior = CommandBehavior.Default, CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, _manager.Connection, parameters, commandTimeout, _manager.Transaction))
            {
                //Get the data reader
                return await command.ExecuteReaderAsync(behavior, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task"/> of <typeparamref name="T"/> from the database
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an errors</param>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <typeparam name="T">The data type to return from data value returned from the query</typeparam>
        /// <returns>Returns the value of the first column in the first row as an instance of <typeparamref name="T"/></returns>
        public virtual async Task<T> GetScalarValueAsync<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, _manager.Connection, parameters, commandTimeout, _manager.Transaction))
            {
                //Return this back to the caller
                return Utilities.GetTypeFromValue<T>(await command.ExecuteScalarAsync(token).ConfigureAwait(false));
            }
        }
        /// <summary>
        /// Utility method for returning an instance of <see cref="IMultiResultReader"/> asynchronously
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The query database parameters that are associated with a query</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="IMultiResultReader"/> object</returns>
        public virtual async Task<IMultiResultReader> GetMultiResultReaderAsync(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            return new MultiResultReader(await GetDbDataReaderAsync(query, queryCommandType, parameters, commandTimeout, CommandBehavior.Default, token), _mapper);
        }
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <exception cref="OperationCanceledException">Thrown in a thread upon cancellation of an operation that the thread was executing</exception>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public virtual async Task<int> ExecuteNonQueryAsync(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, 
            int commandTimeout = 30, CancellationToken token = default)
        {
            //Check if caller has canceled the token
            token.ThrowIfCancellationRequested();

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, _manager.Connection, parameters, commandTimeout, _manager.Transaction))
            {
                //Return this back to the caller
                return await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
            }
        }
        #endregion
    }
}