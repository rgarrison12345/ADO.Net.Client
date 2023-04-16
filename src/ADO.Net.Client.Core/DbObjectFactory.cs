using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// A class that facilitates creating the ADO.NET class objects necessary to query a data store
    /// </summary>
    /// <remarks>
    /// <see cref="DbObjectFactory"/> is a class that is intended to be used at the lowest level of the ADO.NET workflow.  
    /// It creates the objects necessary to query a relational database using the RDBMS providers own driver to do this.
    /// For the .NET framework the providers dll can be within the Global Assembly Cache, and the providers dll can also be used as a dll
    /// contained within the application
    /// </remarks>
    /// <seealso cref="IDbObjectFactory"/>
    public class DbObjectFactory : IDbObjectFactory
    {     
        /// <summary>
        /// An instance of <see cref="DbProviderFactory"/>
        /// </summary>
        protected readonly DbProviderFactory _factory;

#if NET6_0_OR_GREATER
        /// <inheritdoc cref="IDbObjectFactory.CanCreateBatch"/>
        public bool CanCreateBatch => _factory.CanCreateBatch;
#endif
#if !NET462 && !NETSTANDARD2_0
        /// <inheritdoc cref="IDbObjectFactory.CanCreateDataAdapter"/>
        public bool CanCreateDataAdapter => _factory.CanCreateDataAdapter;
        /// <inheritdoc cref="IDbObjectFactory.CanCreateCommandBuilder"/>
        public bool CanCreateCommandBuilder => _factory.CanCreateCommandBuilder;
#endif
        /// <inheritdoc cref="IDbObjectFactory.CanCreateDataSourceEnumerator"/>
        public bool CanCreateDataSourceEnumerator => _factory.CanCreateDataSourceEnumerator;
        
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        public DbObjectFactory(DbProviderFactory factory)
        {
            _factory = factory;
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="providerInvariantName"/>
        /// </summary>
        /// <param name="providerInvariantName">The name of the data provider that the should be used to query a data store</param>
        public DbObjectFactory(string providerInvariantName)
        {
#if !NETSTANDARD2_0
            try
            {
                _factory = DbProviderFactories.GetFactory(providerInvariantName);
            }
            catch (Exception ex)
            {
                _factory = GetProviderFactory(providerInvariantName);
            }
#else
            _factory = GetProviderFactory(providerInvariantName);
#endif
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> </param>
        public DbObjectFactory(DbConnection connection)
        {
#if !NETSTANDARD2_0
            _factory = DbProviderFactories.GetFactory(connection);
#else
            //Get the assembly from the dbconnection type
            _factory = GetProviderFactory(connection.GetType().Assembly);
#endif
        }
#if !NETSTANDARD2_0
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="row"/>
        /// </summary>
        /// <param name="row">An instance of <see cref="DataRow"/> that has the necessary information to create an instance of <see cref="DbProviderFactory"/></param>
        public DbObjectFactory(DataRow row)
        {
            _factory = DbProviderFactories.GetFactory(row);
        }
#endif
#if NET7_0_OR_GREATER
        /// <inheritdoc cref="IDbObjectFactory.GetDbDataSource(string)"/>
        public virtual DbDataSource GetDbDataSource(string connectionString)
        {
            return _factory.CreateDataSource(connectionString);
        }
#endif
#if NET6_0_OR_GREATER
        /// <inheritdoc cref="IDbObjectFactory.GetDbBatch"/>
        public virtual DbBatch GetDbBatch()
        {
            return _factory.CreateBatch();
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbBatchCommand"/>
        public virtual DbBatchCommand GetDbBatchCommand()
        {
            return _factory.CreateBatchCommand();
        }
#endif
        /// <inheritdoc cref="IDbObjectFactory.GetDataSourceEnumerator"/>
        public virtual DbDataSourceEnumerator GetDataSourceEnumerator()
        {
            //Return this back to the caller
            return _factory.CreateDataSourceEnumerator();
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbDataAdapter"/>
        public virtual DbDataAdapter GetDbDataAdapter()
        {
            //Return this back to the caller
            return _factory.CreateDataAdapter();
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbCommandBuilder"/>
        public virtual DbCommandBuilder GetDbCommandBuilder()
        {
            //Return this back to the caller
            return _factory.CreateCommandBuilder();
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbConnectionStringBuilder"/>
        public virtual DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            //Return this back to the caller
            return _factory.CreateConnectionStringBuilder();
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbCommand(CommandType, string, DbConnection, IEnumerable{DbParameter}, int, DbTransaction)"/>
        public virtual DbCommand GetDbCommand(CommandType queryCommandType, string query, DbConnection connection, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, DbTransaction transaction = null)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(connection, transaction, commandTimeout);

            if (parameters != null)
            {
                dCommand.Parameters.AddRange(parameters.ToArray());
            }

            dCommand.CommandType = queryCommandType;
            dCommand.CommandText = query;

            //Return this back to the caller
            return dCommand;
        }
        ///<inheritdoc cref="IDbObjectFactory.GetDbCommand(DbConnection, DbTransaction, int)"/>
        public virtual DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(connection, commandTimeout);

            dCommand.Transaction = transact;

            //Return this back to the caller
            return dCommand;
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbCommand(DbConnection, DbTransaction, int)"/>
        public virtual DbCommand GetDbCommand(DbConnection connection, int commandTimeout)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(commandTimeout);

            dCommand.Connection = connection;

            //Return this back to the caller
            return dCommand;
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbCommand(int)"/>
        public virtual DbCommand GetDbCommand(int commandTimeout)
        {
            DbCommand command = GetDbCommand();

            command.CommandTimeout = commandTimeout;

            //Return this back to the caller
            return command;
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbCommand()"/>
        public virtual DbCommand GetDbCommand()
        {
            DbCommand command = _factory.CreateCommand();

            //Dispose interfaces to clear up any resources used by this instance
            command.Disposed += DbCommand_Disposed;

            //Return this back to the caller
            return command;
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbConnection"/>
        public virtual DbConnection GetDbConnection()
        {
            //Return this back to the caller
            return _factory.CreateConnection();
        }
        /// <inheritdoc cref="IDbObjectFactory.GetDbParameter"/>
        public virtual DbParameter GetDbParameter()
        {
            //Return this back to the caller
            return _factory.CreateParameter();
        }
        /// <summary>
        /// Gets an instance of <see cref="DbProviderFactory"/> based off a .NET drivers <paramref name="providerName"/>, such as System.Data.SqlClientt
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbProviderFactory"/></returns>
        /// <exception cref="ArgumentException">Thrown when the passed in <paramref name="providerName"/> does not have a <see cref="DbProviderFactory"/> type</exception>
        public static DbProviderFactory GetProviderFactory(string providerName)
        {
            //Get the assembly
            return GetProviderFactory(Assembly.Load(new AssemblyName(providerName)));
        }
        /// <summary>
        /// Gets an instance of <see cref="DbProviderFactory"/> based off a .NET driver <see cref="Assembly"/>
        /// Looks for the <see cref="DbProviderFactory"/> within the current <see cref="Assembly"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbProviderFactory"/></returns>
        /// <exception cref="ArgumentException">Thrown when the passed in <paramref name="assembly"/> does not have a <see cref="DbProviderFactory"/> type</exception>
        public static DbProviderFactory GetProviderFactory(Assembly assembly)
        {
            Type providerFactory = assembly.GetTypes().Where(x => x.BaseType == typeof(DbProviderFactory))
                .FirstOrDefault();

            //There's no instance of client factory in this assembly
            if (providerFactory == null)
            {
                throw new ArgumentException($"An instance of {nameof(DbProviderFactory)} was not found in the passed in assembly {assembly.FullName}");
            }

            //Get the field to get the factory instance
            FieldInfo field = providerFactory.GetField("Instance");

            //Get the provider factory
            return (DbProviderFactory)field.GetValue(null);
        }
        /// <summary>
        /// Handles the database command disposed event
        /// </summary>
        /// <param name="sender">The sender as an instance of <see cref="DbCommand"/></param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DbCommand_Disposed(object sender, EventArgs e)
        {
            ((DbCommand)sender).Parameters.Clear();
        }
    }
}