﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract that defines synchronous operations to be performed against a data store
    /// </summary>
    public interface ISynchronousClient
    {
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        DataSet GetDataSet(ISqlQuery query);
        /// <summary>
        /// Utility method for returning a <see cref="DataTable"/> object created from the passed in query
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="DataTable"/> object</returns>
        DataTable GetDataTable(ISqlQuery query);
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        T GetDataObject<T>(ISqlQuery query) where T : class;
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjects<T>(ISqlQuery query) where T : class;
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine, streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjectsStream<T>(ISqlQuery query) where T : class;
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in query
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        DbDataReader GetDbDataReader(ISqlQuery query, CommandBehavior behavior = CommandBehavior.Default);
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        T GetScalarValue<T>(ISqlQuery query);
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetScalarValuesStream<T>(ISqlQuery query);
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of scalar values
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetScalarValues<T>(ISqlQuery query);
        /// <summary>
        /// Gets an instance of <see cref="IMultiResultReader"/>
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="IMultiResultReader"/></returns>
        IMultiResultReader GetMultiResultReader(ISqlQuery query);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure
        /// </summary>
        /// <param name="query">An instance of <see cref="ISqlQuery"/> used to query a data store</param>
        /// <returns>Returns the amount of records affected by the passed in <paramref name="query"/></returns>
        int ExecuteNonQuery(ISqlQuery query);
    }
}