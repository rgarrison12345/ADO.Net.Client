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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// The contract class for a factory class that creates database objects in order to query a database
    /// </summary>
    public interface IDbObjectFactory
    {
        #region Fields/Properties
        /// <summary>
        /// Whether or not the passed in provider is capable of creating a <see cref="DbDataSourceEnumerator"/>
        /// </summary>
        bool CanCreateDataSourceEnumerator { get; }
#if ADVANCED_FEATURES
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbDataAdapter"/>
        /// </summary>
        bool CanCreateDataAdapter { get; }
        /// <summary>
        /// Whether or not this instance is capable of creating a <see cref="DbCommandBuilder"/>
        /// </summary>
        bool CanCreateCommandBuilder { get; }
#endif
        #endregion
        #region Utility Methods
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/></returns>
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
        /// Gets an instance of a <see cref="DbCommand"/> subclass based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a <see cref="DbConnection"/> to a database</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <returns>Returns an instantiated formatted <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout);
        /// <summary>
        /// Instantiates a new isntance of the <see cref="DbCommand"/> subclass based on the provider passed into the class constructor
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="parameters">The <see cref="IEnumerable{DbParameter}"/> of parameters associated with the query parameter</param>
        /// <param name="query">The SQL command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instantiated formatted <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(CommandType queryCommandType, string query, IEnumerable<DbParameter> parameters, DbConnection connection, int commandTimeout, DbTransaction transact = null);
        /// <summary>
        /// Instantiates a new instance of a <see cref="DbConnection"/> subclass based on the specified provider
        /// </summary>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> subclass based on the specified provider</returns>
        DbConnection GetDbConnection();
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> from the passed in <paramref name="values"/>
        /// </summary>
        /// <param name="values">An array of values to be used to create <see cref="DbParameter"/></param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/></returns>
        IEnumerable<DbParameter> GetDbParameters(params object[] values);
        /// <summary>
        /// Create an instance of a <see cref="DbParameter"/> based off of the provider passed into factory
        /// </summary>
        /// <returns>Returns an instantiated <see cref="DbParameter"/> object</returns>
        DbParameter GetDbParameter();
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> subclass based on the specified provider
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> type with information passed into procedure</returns>
        DbParameter GetDbParameter(string parameterName, object parameterValue);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterValue">The value t0 assign to the <see cref="DbParameter"/></param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns></returns>
        DbParameter GetDbParameter(object parameterValue, PropertyInfo info);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter GetDbParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection = ParameterDirection.Input);
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
        DbParameter GetFixedSizeDbParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input);
#endif
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  Should not be set for numeric types.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter GetVariableSizeDbParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input);
        #endregion
    }
}