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
using ADO.Net.Client.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADO.Net.Client
{
    public partial class DbClient
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public override DataSet GetDataSet(ISqlQuery query)
        {
            //Return this back to the caller
            return _executor.GetDataSet(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public override DataTable GetDataTable(ISqlQuery query)
        {
            //Return this back to the caller
            return _executor.GetDataTable(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in query
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/></returns>
        public override DbDataReader GetDbDataReader(ISqlQuery query, CommandBehavior behavior = CommandBehavior.Default)
        {
            //Return this back to the caller
            return _executor.GetDbDataReader(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared, behavior);
        }
        /// <summary>
        /// Utility method for returning a scalar value as an <see cref="object"/> from the database
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        public override T GetScalarValue<T>(ISqlQuery query)
        {
            //Return this back to the caller
            return _executor.GetScalarValue<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public override T GetDataObject<T>(ISqlQuery query) where T : class
        {
            //Return this back to the caller
            return _executor.GetDataObject<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override IEnumerable<T> GetDataObjects<T>(ISqlQuery query) where T : class
        {
            //Return this back to the caller
            return _executor.GetDataObjects<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create to from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override IEnumerable<T> GetDataObjectsStream<T>(ISqlQuery query) where T : class
        {
            //Return this back to the caller
            foreach(T type in _executor.GetDataObjectsStream<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared))
            {
                yield return type;
            }
        }
        /// <summary>
        /// Gets an instance of <see cref="IMultiResultReader" />
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>
        /// Returns an instance of <see cref="IMultiResultReader" />
        /// </returns>
        public override IMultiResultReader GetMultiResultReader(ISqlQuery query)
        {
            return _executor.GetMultiResultReader(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of scalar values
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override IEnumerable<T> GetScalarValues<T>(ISqlQuery query)
        {
            return _executor.GetScalarValues<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        /// <summary>
        /// Gets an instance of <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine streamed from the server
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public override IEnumerable<T> GetScalarValuesStream<T>(ISqlQuery query)
        {
            //Keep going through the enumerator
            foreach(T type in _executor.GetScalarValuesStream<T>(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared))
            {
                yield return type;
            }
        }
        #endregion
        #region Data Modifications
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure
        /// </summary>
        /// <param name="query">An instance of <see cref="ISqlQuery"/> used to query a data store</param>
        /// <returns>Returns the amount of records affected by the passed in <paramref name="query"/></returns>
        public override int ExecuteNonQuery(ISqlQuery query)
        {
            //Return this back to the caller
            return _executor.ExecuteNonQuery(query.QueryText, query.QueryType, query.Parameters, query.CommandTimeout, query.ShouldBePrepared);
        }
        #endregion
    }
}