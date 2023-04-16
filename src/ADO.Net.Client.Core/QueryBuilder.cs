using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Base for classes that build out queries to be executed against a database
    /// </summary>
    /// <seealso cref="IQueryBuilder"/>
    public abstract class QueryBuilder : IQueryBuilder
    {
        private readonly IDbParameterBuilder _parameterBuilder;
        private readonly List<DbParameter> _parameters = new List<DbParameter>();

        /// <summary>
        /// The parameters associated with a query as a <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/>
        /// </summary>
        public IEnumerable<DbParameter> Parameters => _parameters;
       
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder">An  instance of <see cref="IDbParameterBuilder"/></param>
        protected QueryBuilder(IDbParameterBuilder parameterBuilder)
        {
            _parameterBuilder = parameterBuilder;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder">An instance of <see cref="IDbParameterBuilder"/></param>
        /// <param name="parameters">The database parameters associated with a query</param>
        protected QueryBuilder(IDbParameterBuilder parameterBuilder, IEnumerable<DbParameter> parameters) : this(parameterBuilder)
        {
            _parameters.AddRange(parameters);
        }
  
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/>
        /// </summary>
        /// <param name="clearContents">If <c>true</c> when building the query the current <see cref="Parameters"/> will be cleared</param>
        /// <param name="queryText">The Ad-Hoc query or stored procedure name</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        protected ISqlQuery CreateSQLQuery(string queryText, CommandType type, int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = false)
        {
            ISqlQuery query = QueryFactory.CreateSQLQuery(queryText, type, _parameters, commandTimeout, shouldBePrepared);

            if (clearContents)
            {
                _parameters.Clear();
            }

            return query;
        }
        /// <summary>
        /// Adds the passed in parameter to the <see cref="Parameters"/>
        /// </summary>
        /// <param name="param">An instance of the <see cref="DbParameter" /></param>
        /// <exception cref="ArgumentException">Throws argument exception when there are duplicate parameter names</exception>
        public void AddParameter(DbParameter param)
        {
            //Check if this parameter exists before adding to collection
            if (Contains(param.ParameterName))
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
        /// <param name="dbParams">An <see cref="IEnumerable{T}" /> to add to the <see cref="Parameters"/></param>
        public void AddParameterRange(IEnumerable<DbParameter> dbParams)
        {
            //Check incoming parameters for duplicate parameters
            if (dbParams.GroupBy(x => x.ParameterName).Any(g => g.Count() > 1))
            {
                throw new ArgumentException($"The passed in {dbParams} contains duplicate parameter names");
            }

            //Check if any of the items in this IEnumerable already exists in the list by checking parameter name
            foreach (DbParameter dbParam in dbParams)
            {
                //Raise exception here if parameter by name already exists
                if (Contains(dbParam.ParameterName))
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
        /// Checks for a parameter in the <see cref="Parameters"/> with by <paramref name="parameterName"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use when searching the <see cref="Parameters"/></param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parameterName"/> passed into routine is null or empty</exception>
        /// <returns>True if this parameter exists in the parameters collection, false otherwise</returns>
        public bool Contains(string parameterName)
        {
            //Check if this even has a name, caller be using a provider that doesn't support named parameters
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException(nameof(parameterName) + " cannot be null or empty");
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
        /// <exception cref="ArgumentNullException">Thrown when the passed in <paramref name="parameter"/> is <c>null</c></exception>
        public bool Contains(DbParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter) + " cannot be null");
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
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException(nameof(parameterName) + " is null or empty");
            }

            return _parameters.Find(x => x.ParameterName == parameterName);
        }
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the <see cref="Parameters"/> for the current <see cref="DbConnection"/> by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the <see cref="Parameters"/></param>
        /// <returns>Returns true if item was successfully removed, false otherwise if item was not found in the list</returns>
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
        public void SetParameterValue(string parameterName, object value)
        {
            //Get index of this parameter
            int index = _parameters.FindIndex(i => i.ParameterName == parameterName);

            //Do a replace of the parameter
            _parameters[index].Value = value;
        }
    }
}