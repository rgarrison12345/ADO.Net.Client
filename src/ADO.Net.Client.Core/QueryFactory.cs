using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Factory class for creating instances of <see cref="ISqlQuery"/>
    /// </summary>
    public static class QueryFactory
    {
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the <paramref name="parameters"/> and <paramref name="storedProcedureName"/> for a stored procedure
        /// </summary>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> associated with a query</param>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        public static ISqlQuery CreateStoredProcedureQuery(string storedProcedureName, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return CreateSQLQuery(storedProcedureName, CommandType.StoredProcedure, parameters, commandTimeout, shouldBePrepared);
        }
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the <paramref name="parameters"/> and built sql query
        /// </summary>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> associated with a query</param>
        /// <param name="queryText">The Ad-Hoc query</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        public static ISqlQuery CreateAdHocQuery(string queryText, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return CreateSQLQuery(queryText, CommandType.Text, parameters, commandTimeout, shouldBePrepared);
        }
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the <paramref name="parameters"/> and built sql query
        /// </summary>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> associated with a query</param>
        /// <param name="queryText">The Ad-Hoc query or stored procedure name</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        public static ISqlQuery CreateSQLQuery(string queryText, CommandType type, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return new SqlQuery(queryText, type, parameters) { CommandTimeout = commandTimeout, ShouldBePrepared = shouldBePrepared };
        }
    }
}