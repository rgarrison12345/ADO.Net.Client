using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for creating instances of <see cref="DbParameter"/>
    /// </summary>
    public interface IDbParameterBuilder
    {
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterValue">The value to assign to the <see cref="DbParameter"/></param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(object parameterValue, PropertyInfo info);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue);
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> from the passed in <paramref name="values"/>
        /// </summary>
        /// <param name="values">An array of values to be used to create <see cref="DbParameter"/></param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/></returns>
        IEnumerable<DbParameter> CreateParameters(params object[] values);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection = ParameterDirection.Input);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> for a variable length data type
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> for a fixed size parameter
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="scale">The number of decimal places to which the <see cref="DbParameter.Value"/> property is resolved.  The default value is <c>null</c></param>
        /// <param name="precision">The maximum number of digits used to represent the <see cref="DbParameter.Value"/> property.  The default value is <c>null</c></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input);
    }
}