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
#region Using Declarations
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
#endregion

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
        #region Fields/Properties        
        /// <summary>
        /// An instance of <see cref="DbProviderFactory"/>
        /// </summary>
        protected readonly DbProviderFactory _factory;

#if ADVANCED_FEATURES
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbDataAdapter"/>
        /// </summary>
        public bool CanCreateDataAdapter => _factory.CanCreateDataAdapter;
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbCommandBuilder"/>
        /// </summary>
        public bool CanCreateCommandBuilder => _factory.CanCreateCommandBuilder;
#endif
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbDataSourceEnumerator"/>
        /// </summary>
        public bool CanCreateDataSourceEnumerator => _factory.CanCreateDataSourceEnumerator;
        #endregion
        #region Constructors
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
#elif NETSTANDARD2_0
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
        #endregion
        #region Utility Methods
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/></returns>
        public virtual DbDataSourceEnumerator GetDataSourceEnumerator()
        {
            //Return this back to the caller
            return _factory.CreateDataSourceEnumerator();
        }
        /// <summary>
        /// Gets a <see cref="DbDataAdapter"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbDataAdapter"/></returns>
        public virtual DbDataAdapter GetDbDataAdapter()
        {
            //Return this back to the caller
            return _factory.CreateDataAdapter();
        }
        /// <summary>
        /// Gets a <see cref="DbCommandBuilder"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommandBuilder"/></returns>
        public virtual DbCommandBuilder GetDbCommandBuilder()
        {
            //Return this back to the caller
            return _factory.CreateCommandBuilder();
        }
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        public virtual DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            //Return this back to the caller
            return _factory.CreateConnectionStringBuilder();
        }
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        /// <param name="transaction">An instance of <see cref="DbTransaction"/></param>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="parameters">The list of <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> associated with the <paramref name="query"/></param>
        /// <param name="query">The SQL command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
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
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> object</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        public virtual DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(connection, commandTimeout);

            dCommand.Transaction = transact;

            //Return this back to the caller
            return dCommand;
        }
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        public virtual DbCommand GetDbCommand(DbConnection connection, int commandTimeout)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(commandTimeout);

            dCommand.Connection = connection;

            //Return this back to the caller
            return dCommand;
        }
        /// <summary>
        /// Gets an instance of <see cref="DbCommand"/> object
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object</returns>
        public virtual DbCommand GetDbCommand(int commandTimeout)
        {
            DbCommand command = GetDbCommand();

            command.CommandTimeout = commandTimeout;

            //Return this back to the caller
            return command;
        }
        /// <summary>
        /// Gets an instance of <see cref="DbCommand"/> object
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object</returns>
        public virtual DbCommand GetDbCommand()
        {
            DbCommand command = _factory.CreateCommand();

            //Dispose interfaces to clear up any resources used by this instance
            command.Disposed += DbCommand_Disposed;

            //Return this back to the caller
            return command;
        }
        /// <summary>
        /// Instantiates a new instance of the <see cref="DbConnection"/> object based on the specified provider
        /// </summary>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> object based on the specified provider</returns>
        public virtual DbConnection GetDbConnection()
        {
            //Return this back to the caller
            return _factory.CreateConnection();
        }
        /// <summary>
        /// Create an instance of <see cref="DbParameter"/> object based off of the provider passed into factory
        /// </summary>
        /// <returns>Returns an instantiated <see cref="DbParameter"/> object</returns>
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
            Type providerFactory = assembly.GetTypes().Where(x => x.GetTypeInfo().BaseType == typeof(DbProviderFactory)).FirstOrDefault();

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
        #endregion
        #region Helper Methods                
        /// <summary>
        /// Handles the database command disposed event
        /// </summary>
        /// <param name="sender">The sender as an instance of <see cref="DbCommand"/></param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DbCommand_Disposed(object sender, EventArgs e)
        {
            ((DbCommand)sender).Parameters.Clear();
        }
        #endregion
    }
}