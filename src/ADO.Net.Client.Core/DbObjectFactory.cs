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
using System.Collections;
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
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly IDbParameterFormatter _dbParameterFormatter;

#if !NET45 && !NET461 && !NETSTANDARD2_0
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbDataAdapter"/>
        /// </summary>
        public bool CanCreateDataAdapter
        {
            get
            {
                return _dbProviderFactory.CanCreateDataAdapter;
            }
        }
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbCommandBuilder"/>
        /// </summary>
        public bool CanCreateCommandBuilder
        {
            get
            {
                return _dbProviderFactory.CanCreateCommandBuilder;
            }
        }
#endif
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbDataSourceEnumerator"/>
        /// </summary>
        public bool CanCreateDataSourceEnumerator
        {
            get
            {
                //Return this back to the caller
                return _dbProviderFactory.CanCreateDataSourceEnumerator;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        public DbObjectFactory(DbProviderFactory factory) : this(factory, new DbParameterFormatter())
        {
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="factory"/>
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        public DbObjectFactory(DbProviderFactory factory, IDbParameterFormatter formatter) : this(formatter)
        {
            _dbProviderFactory = factory;
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="providerInvariantName"/>
        /// </summary>
        /// <param name="providerInvariantName">The name of the data provider that the should be used to query a data store</param>
        public DbObjectFactory(string providerInvariantName) : this(providerInvariantName, new DbParameterFormatter())
        {

        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="providerInvariantName"/>
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="providerInvariantName">The name of the data provider that the should be used to query a data store</param>
        public DbObjectFactory(string providerInvariantName, IDbParameterFormatter formatter) : this(formatter)
        {
#if !NETSTANDARD2_0
            try
            {
                _dbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
            }
            catch (Exception ex)
            {
                _dbProviderFactory = GetProviderFactory(providerInvariantName);
            }
#else
            _dbProviderFactory = GetProviderFactory(providerInvariantName);
#endif
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> </param>
        public DbObjectFactory(DbConnection connection) : this(connection, new DbParameterFormatter())
        {
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="connection"/>
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="connection">An instance of <see cref="DbConnection"/> </param>
        public DbObjectFactory(DbConnection connection, IDbParameterFormatter formatter) : this(formatter)
        {
#if !NETSTANDARD2_0
            _dbProviderFactory = DbProviderFactories.GetFactory(connection);
#elif NETSTANDARD2_0
            //Get the assembly from the dbconnection type
            _dbProviderFactory = GetProviderFactory(connection.GetType().Assembly);
#endif
        }
#if !NETSTANDARD2_0
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="row"/>
        /// </summary>
        /// <param name="row">An instance of <see cref="DataRow"/> that has the necessary information to create an instance of <see cref="DbProviderFactory"/></param>
        public DbObjectFactory(DataRow row) : this(row, new DbParameterFormatter())
        {
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="row"/>
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="row">An instance of <see cref="DataRow"/> that has the necessary information to create an instance of <see cref="DbProviderFactory"/></param>
        public DbObjectFactory(DataRow row, IDbParameterFormatter formatter) : this(formatter)
        {
            _dbProviderFactory = DbProviderFactories.GetFactory(row);
        }
#endif
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="dbParameterFormatter"/>
        /// </summary>
        /// <param name="dbParameterFormatter">An instance of <see cref="DbParameterFormatter"/> to help format <see cref="DbParameter"/></param>
        private DbObjectFactory(IDbParameterFormatter dbParameterFormatter)
        {
            _dbParameterFormatter = dbParameterFormatter;
        }
        #endregion
        #region Utility Methods
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/></returns>
        public DbDataSourceEnumerator GetDataSourceEnumerator()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateDataSourceEnumerator();
        }
        /// <summary>
        /// Gets a <see cref="DbDataAdapter"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbDataAdapter"/></returns>
        public DbDataAdapter GetDbDataAdapter()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateDataAdapter();
        }
        /// <summary>
        /// Gets a <see cref="DbCommandBuilder"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommandBuilder"/></returns>
        public DbCommandBuilder GetDbCommandBuilder()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateCommandBuilder();
        }
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        public DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateConnectionStringBuilder();
        }
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        ///<param name="trasaction">An instance of <see cref="DbTransaction"/></param>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="parameters">The list of <see cref="IEnumerable{DbParameter}"/> associated with the query parameter</param>
        /// <param name="query">The SQL command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        public DbCommand GetDbCommand(CommandType queryCommandType, string query, IEnumerable<DbParameter> parameters, DbConnection connection, int commandTimeout, DbTransaction trasaction = null)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(connection, trasaction, commandTimeout);

            if (parameters != null)
            {
                //Set query and command type
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
        public DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(commandTimeout);

            //Set query and command type
            dCommand.Connection = connection;
            dCommand.Transaction = transact;

            //Return this back to the caller
            return dCommand;
        }
        /// <summary>
        /// Gets an instance of <see cref="DbCommand"/> object
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object</returns>
        public DbCommand GetDbCommand(int commandTimeout)
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
        public DbCommand GetDbCommand()
        {
            DbCommand command = _dbProviderFactory.CreateCommand();

            //Dispose interfaces to clear up any resources used by this instance
            command.Disposed += DbCommand_Disposed;

            //Return this back to the caller
            return command;
        }
        /// <summary>
        /// Instantiates a new instance of the <see cref="DbConnection"/> object based on the specified provider
        /// </summary>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> object based on the specified provider</returns>
        public DbConnection GetDbConnection()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateConnection();
        }
#if !NET45
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="scale">The number of decimal places to which the <see cref="DbParameter.Value"/> property is resolved.  The default value is <c>null</c></param>
        /// <param name="precision">The maximum number of digits used to represent the <see cref="DbParameter.Value"/> property.  The default value is <c>null</c></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetFixedSizeDbParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter(parameterName, parameterValue, dataType, paramDirection);

            //Check for values
            if (precision.HasValue == true)
            {
                parameter.Precision = precision.Value;
            }
            if (scale.HasValue == true)
            {
                parameter.Scale = scale.Value;
            }

            //Return this back to the caller
            return parameter;
        }
#endif
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when the <paramref name="paramDirection"/> is <see cref="ParameterDirection.Output"/> and the <paramref name="size"/> is <c>null</c></exception>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetVariableSizeDbParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter(parameterName, parameterValue, dataType, paramDirection);

            //Check for value
            if (size.HasValue == true)
            {
                parameter.Size = size.Value;
            }

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetDbParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter(parameterName, parameterValue);

            //Set parameter properties
            parameter.DbType = dataType;
            parameter.Direction = paramDirection;

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterValue">The value t0 assign to the <see cref="DbParameter"/></param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns></returns>
        public DbParameter GetDbParameter(object parameterValue, PropertyInfo info)
        {
            DbParameter parameter = GetDbParameter();

            _dbParameterFormatter.MapDbParameter(parameter, parameterValue, info);

            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetDbParameter(string parameterName, object parameterValue)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter();

            parameter.Value = parameterValue ?? DBNull.Value;
            parameter.ParameterName = _dbParameterFormatter.MapParameterName(parameterName);

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> from the passed in <paramref name="values"/>
        /// </summary>
        /// <param name="values">An array of values to be used to create <see cref="DbParameter"/></param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/></returns>
        public IEnumerable<DbParameter> GetDbParameters(params object[] values)
        {
            List<DbParameter> result = new List<DbParameter>();

            void ProcessArg(object value)
            {
                //Check if this is an enumerable object 
                if (value.IsEnumerable() == true)
                {
                    //Go through each item in the enumerable
                    foreach (var singleArg in value as IEnumerable)
                    {
                        //Recurse the call
                        ProcessArg(singleArg);
                    }
                }
                else if (value is DbParameter parameter)
                {
                    result.Add(parameter);
                }
                else
                {
                    Type type = value.GetType();

                    //Can't have plain string or value types
                    if (type.IsValueType || type == typeof(string))
                    {
                        throw new ArgumentException($"Value type or string passed as Parameter object: {value}");
                    }

                    //We only want properties where we can read a value
                    IEnumerable<PropertyInfo> readableProps = type.GetProperties().Where(p => p.CanRead == true);

                    //Loop through each property
                    foreach (PropertyInfo prop in readableProps)
                    {
                        result.Add(GetDbParameter(prop.GetValue(value, null), prop));
                    }
                }
            }

            //Go through all items in the params array
            foreach (object arg in values)
            {
                ProcessArg(arg);
            }

            //Nothing to return here
            return result;
        }
        /// <summary>
        /// Create an instance of <see cref="DbParameter"/> object based off of the provider passed into factory
        /// </summary>
        /// <returns>Returns an instantiated <see cref="DbParameter"/> object</returns>
        public DbParameter GetDbParameter()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateParameter();
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
        /// Databases the command disposed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DbCommand_Disposed(object sender, EventArgs e)
        {
            ((DbCommand)sender).Parameters.Clear();
        }
        #endregion
    }
}