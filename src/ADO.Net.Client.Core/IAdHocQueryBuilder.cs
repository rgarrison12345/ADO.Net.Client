using System.Collections.Generic;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for creating an Ad-hoc query
    /// </summary>
    public interface IAdHocQueryBuilder
    {
        /// <summary>
        /// The query command text to execute against the data store
        /// </summary>
        /// <value>
        /// The <see cref="string"/> value of ad-hoc query
        /// </value>
        string QueryText { get; }
       
        /// <summary>
        /// Clears the underlying SQL query being created by this instance
        /// </summary>
        void ClearSQL();
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        void Append(string sql);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameter">The database parameter associated with this SQL statement</param>
        void Append(string sql, DbParameter parameter);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="paramerterName">Name of the paramerter.</param>
        /// <param name="parmaeterValue">The parmaeter value.</param>
        void Append(string sql, string paramerterName, object parmaeterValue);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built.
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameters">The database parameters associated with this query</param>
        void Append(string sql, params object[] parameters);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameters">The database parameters associated with this query</param>
        void Append(string sql, IEnumerable<DbParameter> parameters);
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the existing parameters and <see cref="QueryText"/>
        /// </summary>
        /// <param name="clearContents">If <c>true</c> when building the query the current parameters and <see cref="QueryText"/> will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        ISqlQuery CreateAdHocQuery(int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true);
    }
}
