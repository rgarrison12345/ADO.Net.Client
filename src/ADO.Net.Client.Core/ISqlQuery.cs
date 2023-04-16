using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for a transfer object that contains the information about a query to use when querying a datastore
    /// </summary>
    public interface ISqlQuery
    {   
        /// <summary>
        /// Indicates if the current <see cref="QueryText"/> needs to be prepared (or compiled) version of the command on the data source.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the <see cref="QueryText"/> needs to be prepared; otherwise, <c>false</c>.
        /// </value>
        bool ShouldBePrepared { get; }
        /// <summary>
        /// Represents how a command should be interpreted by the data provider
        /// </summary>
        CommandType QueryType { get; }
        /// <summary>
        /// The query command text or name of stored procedure to execute against the data store
        /// </summary>
        string QueryText { get; }
        /// <summary>
        /// The database parameters that are associated with a query
        /// </summary>
        IEnumerable<DbParameter> Parameters { get; }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.
        /// </summary>
        int CommandTimeout { get; }
    }
}