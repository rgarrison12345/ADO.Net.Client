using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// The contract for a factory class that creates database objects in order to query a database
    /// </summary>
    public interface IDbObjectFactory
    {
        /// <summary>
        /// Whether or not the passed in provider is capable of creating a <see cref="DbDataSourceEnumerator"/>
        /// </summary>
        bool CanCreateDataSourceEnumerator { get; }
#if !NET462 && !NETSTANDARD2_0
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbDataAdapter"/>
        /// </summary>
        bool CanCreateDataAdapter { get; }
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbCommandBuilder"/>
        /// </summary>
        bool CanCreateCommandBuilder { get; }
#endif
#if NET6_0_OR_GREATER
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbBatch"/>
        /// </summary>
        bool CanCreateBatch { get; }
#endif
      
#if NET7_0_OR_GREATER
        /// <summary>
        /// Gets a <see cref="DbDataSource"/> based off the provider passed into class
        /// </summary>
        /// <param name="connectionString">The connection string associated with the data source</param>
        /// <returns>An instance of <see cref="DbDataSource"/></returns>
        DbDataSource GetDbDataSource(string connectionString);
#endif
#if NET6_0_OR_GREATER
        /// <summary>
        /// Gets a <see cref="DbBatch"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbBatch"/></returns>
        DbBatch GetDbBatch();
        /// <summary>
        /// Gets a <see cref="DbBatchCommand"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbBatchCommand"/></returns>
        DbBatchCommand GetDbBatchCommand();
#endif
        /// <summary>
        /// Gets an instance of <see cref="DbDataSourceEnumerator"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbDataSourceEnumerator"/></returns>
        DbDataSourceEnumerator GetDataSourceEnumerator();
        /// <summary>
        /// Gets a <see cref="DbCommandBuilder"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommandBuilder"/></returns>
        DbCommandBuilder GetDbCommandBuilder();
        /// <summary>
        /// Gets a <see cref="DbDataAdapter"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbDataAdapter"/></returns>
        DbDataAdapter GetDbDataAdapter();
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        DbConnectionStringBuilder GetDbConnectionStringBuilder();
        /// <summary>
        /// Gets an instance of a <see cref="DbCommand"/> object
        /// </summary>
        /// <returns>Returns an instance of a <see cref="DbCommand"/> Object</returns>
        DbCommand GetDbCommand();
        /// <summary>
        /// Gets an instance of a <see cref="DbCommand"/> object
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> Object</returns>
        DbCommand GetDbCommand(int commandTimeout);
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(DbConnection connection, int commandTimeout);
        /// <summary>
        /// Gets an instance of a <see cref="DbCommand"/> subclass based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a <see cref="DbConnection"/> to a database</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <returns>Returns an instantiated formatted <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout);
        /// <summary>
        /// Instantiates a new instance of the <see cref="DbCommand"/> subclass based on the provider passed into the class constructor
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <param name="transaction">An instance of <see cref="DbTransaction"/></param>
        /// <param name="parameters">The <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> associated with the query parameter</param>
        /// <param name="query">The SQL command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instantiated formatted <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(CommandType queryCommandType, string query, DbConnection connection, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, DbTransaction transaction = null);
        /// <summary>
        /// Instantiates a new instance of a <see cref="DbConnection"/> subclass based on the specified provider
        /// </summary>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> subclass based on the specified provider</returns>
        DbConnection GetDbConnection();
        /// <summary>
        /// Create an instance of a <see cref="DbParameter"/> based off of the provider passed into factory
        /// </summary>
        /// <returns>Returns an instantiated <see cref="DbParameter"/> object</returns>
        DbParameter GetDbParameter();
    }
}