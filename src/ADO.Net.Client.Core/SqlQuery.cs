using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Transfer object that contains the information about a query to use when querying a datastore
    /// </summary>
    /// <seealso cref="ISqlQuery"/>
    public class SqlQuery : ISqlQuery
    {
        /// <summary>
        /// Indicates if the current <see cref="QueryText"/> needs to be prepared (or compiled) version of the command on the data source.
        /// </summary>
        public bool ShouldBePrepared { get; internal set; }
        /// <summary>
        /// Represents how a command should be interpreted by the data provider
        /// </summary>
        public CommandType QueryType { get; private set; }
        /// <summary>
        /// The query command text or name of stored procedure to execute against the data store
        /// </summary>
        public string QueryText { get; private set; }
        /// <summary>
        /// The query database parameters that are associated with a query
        /// </summary>
        public IEnumerable<DbParameter> Parameters { get; private set; }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.  Default is 30 seconds
        /// </summary>
        public int CommandTimeout { get; internal set; } = 30;
        
        /// <summary>
        /// Instantiates an instance with <paramref name="query"/>, <paramref name="type"/>, and <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> that are associated with a <paramref name="query"/></param>
        internal SqlQuery(string query, CommandType type, IEnumerable<DbParameter> parameters)
        {
            QueryText = query;
            QueryType = type;
            Parameters = parameters;
        }
    }
}