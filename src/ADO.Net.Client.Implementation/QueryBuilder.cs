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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
#endregion

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Utility class that builds out queries to be exectued against a database
    /// </summary>
    /// <seealso cref="IQueryBuilder"/>
    public class QueryBuilder : IQueryBuilder
    {
        #region Fields/Properties
        private readonly List<DbParameter> _parameters;
        private readonly StringBuilder _sqlQuery;
        private readonly IDbObjectFactory _factory;

        /// <summary>
        /// The database parameters associated with a query
        /// </summary>
        /// <value>
        /// The parameters associated with a query as a <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/>
        /// </value>
        public IEnumerable<DbParameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }
        /// <summary>
        /// The query command text or name of stored procedure to execute against the data store
        /// </summary>
        /// <value>
        /// The <see cref="string"/> value of stored procedure or ad-hoc query
        /// </value>
        public string QueryText
        {
            get
            {
                return _sqlQuery.ToString();
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="queryText">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="parameters">The database parameters associated with <paramref name="queryText"/></param>
        public QueryBuilder(string queryText, IEnumerable<DbParameter> parameters, IDbObjectFactory factory) : this(queryText, factory)
        {
            AddParameterRange(parameters);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="queryText">The query command text or name of stored procedure to execute against the data store</param>
        public QueryBuilder(string queryText, IDbObjectFactory factory) : this(factory)
        {
            Append(queryText);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="factory"></param>
        public QueryBuilder(IDbObjectFactory factory)
        {
            _factory = factory;
            _parameters = new List<DbParameter>();
            _sqlQuery = new StringBuilder();
        }
        #endregion
        #region SQL Methods
        /// <summary>
        /// Clears the underlying SQL query being created by this instance
        /// </summary>
        public void ClearSQL()
        {
            _sqlQuery.Clear();
        }
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        public void Append(string sql)
        {
            _sqlQuery.Append(sql);
        }
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameter">The database parameter associated with this SQL statement</param>
        public void Append(string sql, DbParameter parameter)
        {
            Append(sql);
            AddParameter(parameter);
        }
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameters">The database parameters associated with this query</param>
        public void Append(string sql, IEnumerable<DbParameter> parameters)
        {
            Append(sql);
            AddParameterRange(parameters);
        }
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="paramerterName">Name of the paramerter.</param>
        /// <param name="parmaeterValue">The parmaeter value.</param>
        public void Append(string sql, string paramerterName, object parmaeterValue)
        {
            Append(sql);
            AddParameter(_factory.GetDbParameter(paramerterName, parmaeterValue));
        }
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built.
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameters">The database parameters associated with this query</param>
        public void Append(string sql, params object[] parameters)
        {
            Append(sql);
            AddParameterRange(parameters);
        }
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the existing <see cref="Parameters"/> and built sql query
        /// </summary>
        /// <param name="clearContents">If <c>true</c> when building the query the current <see cref="Parameters"/> and <see cref="QueryText"/> will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        public ISqlQuery CreateSQLQuery(CommandType type, int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true)
        {
            SQLQuery query = new SQLQuery(_sqlQuery.ToString(), type, _parameters) { CommandTimeout = commandTimeout, ShouldBePrepared = shouldBePrepared };

            //Check if we need to clear any state
            if (clearContents == true)
            {
                //Clear out any existing state
                _parameters.Clear();
                _sqlQuery.Clear();
            }

            return query;
        }
        #endregion
        #region Parameter Methods        
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="param">An instance of the <see cref="T:System.Data.Common.DbParameter" /> object, that is created the by the caller</param>
        /// <exception cref="ArgumentException">Throws argument exception when there are duplicate parameter names</exception>
        public void AddParameter(DbParameter param)
        {
            //Check if this parameter exists before adding to collection
            if (Contains(param.ParameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {param.ParameterName} already exists", nameof(param));
            }
            else
            {
                //Add this parameter
                _parameters.Add(param);
            }
        }
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as an <see cref="object"/></param>
        public void AddParameter(string parameterName, object parameterValue)
        {
            AddParameter(_factory.GetDbParameter(parameterName, parameterValue));
        }
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="parameters">The parameters that are associated with a database query</param>
        public void AddParameterRange(params object[] parameters)
        {
            AddParameterRange(_factory.GetDbParameters(parameters));
        }
        /// <summary>
        /// Adds an <see cref="IEnumerable{T}" /> of <see cref="DbParameter"/> objects to the helpers underlying db parameter collection
        /// </summary>
        /// <exception cref="ArgumentException">Throws argument exception when there are duplicate parameter names</exception>
        /// <param name="dbParams">An <see cref="IEnumerable{T}" /> to add to the underlying db parameter collection for the connection</param>
        public void AddParameterRange(IEnumerable<DbParameter> dbParams)
        {
            //Check incoming parameters for duplicate parameters
            if (dbParams.GroupBy(x => x.ParameterName).Any(g => g.Count() > 1) == true)
            {
                throw new ArgumentException($"The passed in {dbParams} contains duplicate parameter names");
            }

            //Check if any of the items in this IEnumerable already exists in the list by checking parameter name
            foreach (DbParameter dbParam in dbParams)
            {
                //Raise exception here if parameter by name already exists
                if (Contains(dbParam.ParameterName) == true)
                {
                    throw new ArgumentException($"Parameter with name {dbParam.ParameterName} already exists");
                }
            }

            _parameters.AddRange(dbParams);
        }
        /// <summary>
        /// Clears all parameters from the parameters collection
        /// </summary>
        public void ClearParameters()
        {
            _parameters.Clear();
        }
        /// <summary>
        /// Checks for a paremeter in the parameters list with the passed in name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use when searching the Parameters list</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parameterName"/> passed into routine is null or empty</exception>
        /// <returns>True if this parameter exists in the parameters collection, false otherwise</returns>
        public bool Contains(string parameterName)
        {
            //Check if this even has a name, caller be using a provider that doesn't support named parameters
            if (string.IsNullOrWhiteSpace(parameterName) == true)
            {
                throw new ArgumentException(nameof(parameterName) + "cannot be null or empty");
            }

            //Return this back to the caller
            return !(_parameters.Find(x => x.ParameterName == parameterName) == null);
        }
        /// <summary>
        /// Determines whether this instance contains the passed in <paramref name="parameter"/>
        /// </summary>
        /// <param name="parameter">An instance of <see cref="DbParameter"/> that may be associated with this instance</param>
        /// <returns>
        ///   <c>true</c> if this instance contains the passed in <paramref name="parameter"/> otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Contains(DbParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException(nameof(parameter) + "cannot be null");
            }

            //Return this back to the caller
            return _parameters.Contains(parameter);
        }
        /// <summary>
        /// Retrieves a <see cref="DbParameter"/> object by using the passed in parameter name
        /// </summary>
        /// <exception cref="ArgumentException">Throws when the passed in <paramref name="parameterName"/> is <c>null</c> or <see cref="string.Empty"/></exception>
        /// <exception cref="InvalidOperationException">Thrown when the passed in parameter name is not present in the parameters collection</exception>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>The specified <see cref="DbParameter"/> object from the parameters collection</returns>
        public DbParameter GetParameter(string parameterName)
        {
            //Check for null or empty
            if (string.IsNullOrWhiteSpace(parameterName) == true)
            {
                throw new ArgumentException(nameof(parameterName) + " is null or empty");
            }

            return _parameters.Find(x => x.ParameterName == parameterName);
        }
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection for the current <see cref="DbConnection"/> by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        public bool RemoveParameter(string parameterName)
        {
            //Return this back to the caller
            return _parameters.Remove(_parameters.Find(x => x.ParameterName == parameterName));
        }
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> with an existing <see cref="DbParameter.ParameterName"/>
        /// </summary>
        /// <param name="parameterName">The index as a <c>string</c> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        public void ReplaceParameter(string parameterName, DbParameter param)
        {
            //Get index of this parameter
            int index = _parameters.FindIndex(i => i.ParameterName == parameterName);

            //Do a replace of the parameter
            _parameters[index] = param;
        }
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="parameterName"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        public void SetParamaterValue(string parameterName, object value)
        {
            //Get index of this parameter
            int index = _parameters.FindIndex(i => i.ParameterName == parameterName);

            //Do a replace of the parameter
            _parameters[index].Value = value;
        }
        #endregion
    }
}