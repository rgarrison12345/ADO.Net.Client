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
using System.Linq;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Utility class that builds out queries to be exectued against a database
    /// </summary>
    /// <seealso cref="IQueryBuilder"/>
    public class QueryBuilder : IQueryBuilder
    {
        #region Fields/Properties
        private readonly List<DbParameter> _parameters = new List<DbParameter>();
        private readonly IDbParameterBuilder _parameterBuilder;

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
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="paramBuilder"></param>
        public QueryBuilder(IDbParameterBuilder paramBuilder)
        {
            _parameterBuilder = paramBuilder;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="paramBuilder"></param>
        /// <param name="parameters">The database parameters associated with a query</param>
        public QueryBuilder(IDbParameterBuilder paramBuilder, IEnumerable<DbParameter> parameters) : this(paramBuilder)
        {
            AddParameterRange(parameters);
        }
        #endregion
        #region SQL Methods
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the existing <see cref="Parameters"/> and built sql query
        /// </summary>
        /// <param name="queryText">The Ad-Hoc query or stored procedure name</param>
        /// <param name="clearContents">If <c>true</c> when building the query the current <see cref="Parameters"/> will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        public SqlQuery CreateSQLQuery(string queryText, CommandType type, int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true)
        {
            SqlQuery query = new SqlQuery(queryText, type, _parameters) { CommandTimeout = commandTimeout, ShouldBePrepared = shouldBePrepared };

            //Check if we need to remove the parameters
            if (clearContents == true)
            {
                _parameters.Clear();
            }

            return query;
        }
        #endregion
        #region Parameter Methods        
        /// <summary>
        /// Adds the passed in parameter to the <see cref="Parameters"/>
        /// </summary>
        /// <param name="param">An instance of the <see cref="DbParameter" /></param>
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
        /// Adds the passed in parameter to the <see cref="Parameters"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as an <see cref="object"/></param>
        public void AddParameter(string parameterName, object parameterValue)
        {
            AddParameter(_parameterBuilder.CreateParameter(parameterName, parameterValue));
        }
        /// <summary>
        /// Adds the passed in parameter to the <see cref="Parameters"/>
        /// </summary>
        /// <param name="parameters">The parameters that are associated with a database query</param>
        public void AddParameterRange(params object[] parameters)
        {
            AddParameterRange(_parameterBuilder.CreateParameters(parameters));
        }
        /// <summary>
        /// Adds an <see cref="IEnumerable{T}" /> of <see cref="DbParameter"/> to the <see cref="Parameters"/>
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
        /// Clears all parameters from the <see cref="Parameters"/>
        /// </summary>
        public void ClearParameters()
        {
            _parameters.Clear();
        }
        /// <summary>
        /// Checks for a paremeter in the <see cref="Parameters"/> with by <paramref name="parameterName"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use when searching the <see cref="Parameters"/></param>
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
        /// <returns>The specified <see cref="DbParameter"/> object from the <see cref="Parameters"/></returns>
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
        /// Removes a <see cref="DbParameter"/> from the <see cref="Parameters"/> for the current <see cref="DbConnection"/> by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the <see cref="Parameters"/></param>
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