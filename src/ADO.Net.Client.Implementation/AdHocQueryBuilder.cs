using ADO.Net.Client.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Class for creating Ad-hoc queries
    /// </summary>
    /// <see cref="QueryBuilder"/>
    /// <see cref="IAdHocQueryBuilder"/>
    public class AdHocQueryBuilder : QueryBuilder, IAdHocQueryBuilder
    {
        private readonly StringBuilder _sqlQuery = new StringBuilder();

        /// <summary>
        /// The query command text to execute against the data store
        /// </summary>
        /// <value>
        /// The <see cref="string"/> value of ad-hoc query
        /// </value>
        public string QueryText => _sqlQuery.ToString();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocQueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder">An instance of <see cref="IDbParameterBuilder"/></param>
        public AdHocQueryBuilder(IDbParameterBuilder parameterBuilder) : base(parameterBuilder)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocQueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder">An instance of <see cref="IDbParameterBuilder"/></param>
        /// <param name="parameters">The database parameters associated with a query</param>
        public AdHocQueryBuilder(IDbParameterBuilder parameterBuilder, IEnumerable<DbParameter> parameters) : base(parameterBuilder, parameters)
        {
        }
        
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
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public void Append(string sql, string parameterName, object parameterValue)
        {
            Append(sql);
            AddParameter(parameterName, parameterValue);
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
        /// Create an instance of <see cref="ISqlQuery"/> using the existing parameters and <see cref="QueryText"/>
        /// </summary>
        /// <param name="clearContents">If <c>true</c> when building the query the current parameters and <see cref="QueryText"/> will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        public ISqlQuery CreateAdHocQuery(int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true)
        {
            ISqlQuery query = CreateSQLQuery(_sqlQuery.ToString(), CommandType.Text, commandTimeout, shouldBePrepared, clearContents);

            //Check if we need to clear any state
            if (clearContents)  
            {
                _sqlQuery.Clear();
            }

            return query;
        }
    }
}