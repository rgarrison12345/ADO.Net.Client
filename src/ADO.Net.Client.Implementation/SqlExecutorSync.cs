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
#region Using Declarations
using ADO.Net.Client.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Implementation
{
    public partial class SqlExecutor
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The parameters that are associated with a database query</param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            //Wrap this automatically to dispose of resources
            using (DbDataAdapter adap = _factory.GetDbDataAdapter())
            {
                //Wrap this automatically to dispose of resources
                using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, parameters, _manager.Connection, commandTimeout, _manager.Transaction))
                {
                    DataSet set = new DataSet();

                    //Check if we should be prepared
                    if(shouldBePrepared == true)
                    {
                        command.Prepare();
                    }

                    //Fill out the dataset
                    adap.SelectCommand = command;
                    adap.Fill(set);

                    //Return this back to the caller
                    return set;
                }
            }
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            //Return this back to the caller
            using (DbDataReader reader = GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared))
            {
                DataTable dt = new DataTable();

                //Load in the result set
                dt.Load(reader);

                //Return this back to the caller
                return dt;
            }
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The database parameters associated with a database query</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the <typeparamref name="T"/> if a record is not found</returns>
        public T GetDataObject<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false) where T : class
        {
            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared, CommandBehavior.SingleRow))
            {
                //Check if the reader has rows
                if (reader.HasRows == true)
                {
                    //Move to the first record in the result set
                    reader.Read();

                    //Return this back to the caller
                    return _mapper.MapRecord<T>(reader);
                }
                else
                {
                    return default;
                }
            }
        }
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjects<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false) where T : class
        {
            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared, CommandBehavior.SingleResult))
            {
                return _mapper.MapResultSet<T>(reader);
            }
        }
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine as an iterator function
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjectsStream<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false) where T : class
        {
            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared, CommandBehavior.SingleResult))
            {
                //Keep going through each item
                foreach (T type in _mapper.MapResultSetStream<T>(reader))
                {
                    //Return this back to the caller
                    yield return type;
                }

                yield break;
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.CloseConnection"/></param>
        /// <param name="parameters">The database parameters associated with a query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader</returns>
        public DbDataReader GetDbDataReader(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false, CommandBehavior behavior = CommandBehavior.CloseConnection)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, parameters, _manager.Connection, commandTimeout, _manager.Transaction))
            {
                //Check if we should be prepared
                if (shouldBePrepared == true)
                {
                    command.Prepare();
                }

                //Return this back to the caller
                return command.ExecuteReader(behavior);
            }
        }
        /// <summary>
        /// Utility method for returning an <see cref="IEnumerable{T}"/> of scalar values from the database
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <typeparam name="T">The data type to return from data value returned from the query</typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of the value of the first column in the result set as an instance of <typeparamref name="T"/></returns>
        public IEnumerable<T> GetScalarValues<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            List<T> returnValues = new List<T>();

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared, CommandBehavior.SingleResult))
            {
                while(reader.Read() == true)
                {
                    returnValues.Add(reader.GetFieldValue<T>(0));
                }
            }
             
            return returnValues;
        }
        /// <summary>
        /// Utility method for returning an <see cref="IEnumerable{T}"/> of scalar values streamed from the database
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <typeparam name="T">The data type to return from data value returned from the query</typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of the value of the first column in the result set as an instance of <typeparamref name="T"/></returns>
        public IEnumerable<T> GetScalarValuesStream<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared, CommandBehavior.SingleResult))
            {
                //Keep reading through the results
                while (reader.Read() == true)
                {
                    yield return reader.GetFieldValue<T>(0);
                }
            }

            yield break;
        }
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <typeparam name="T">The data type to return from data value returned from the query</typeparam>
        /// <returns>Returns the value of the first column in the first row as an instance of <typeparamref name="T"/></returns>
        public T GetScalarValue<T>(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, parameters, _manager.Connection, commandTimeout, _manager.Transaction))
            {
                //Check if we should be prepared
                if (shouldBePrepared == true)
                {
                    command.Prepare();
                }

                //Return this back to the caller
                return Utilities.GetTypeFromValue<T>(command.ExecuteScalar());
            }
        }
        /// <summary>
        /// Utility method for returning an instance of <see cref="IMultiResultReader"/>
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The query database parameters that are associated with a query</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="IMultiResultReader"/> object</returns>
        public IMultiResultReader GetMultiResultReader(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return new MultiResultReader(GetDbDataReader(query, queryCommandType, parameters, commandTimeout, shouldBePrepared), _mapper);
        }
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="shouldBePrepared">Indicates if the current <paramref name="query"/> needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="parameters">The parameters associated with a database query</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteNonQuery(string query, CommandType queryCommandType, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbCommand command = _factory.GetDbCommand(queryCommandType, query, parameters, _manager.Connection, commandTimeout, _manager.Transaction))
            {
                //Check if we should be prepared
                if (shouldBePrepared == true)
                {
                    command.Prepare();
                }

                //Return the amount of records affected by this query back to the caller
                return command.ExecuteNonQuery();
            }
        }
        #endregion
        #region Helper Methods
        #endregion
    }
}