using System.Data;
using System.Data.Common;
using System.Reflection;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract the defines the behavior of a <see cref="DbParameter"/> formatter class
    /// </summary>
    public interface IDbParameterFormatter
    {
        /// <summary>
        /// Gets a value indicating whether this instance has native unique identifier support.  Defaults to <c>true</c>
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has native unique identifier support; otherwise, <c>false</c>.
        /// </value>
        bool HasNativeGuidSupport { get; }
        /// <summary>
        /// Gets or sets the parameter name prefix.
        /// </summary>
        /// <value>
        /// The parameter name prefix.
        /// </value>
        string ParameterNamePrefix { get; }
   
        /// <summary>
        /// Maps a parameter name from an instance of <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns the name of a parameter</returns>
        string MapParameterName(PropertyInfo info);
        /// <summary>
        /// Maps the name of the parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>Returns the name of a parameter</returns>
        string MapParameterName(string parameterName);
        /// <summary>
        /// Maps an instance of a <see cref="DbParameter"/> using the passed in <paramref name="info"/> <paramref name="parameterValue"/>
        /// </summary>
        /// <param name="parameter">An instance of <see cref="DbParameter"/></param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        void MapDbParameter(DbParameter parameter, object parameterValue, PropertyInfo info);
        /// <summary>
        /// Maps the type value of a <see cref="DbType"/> from an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns a value of <see cref="DbType"/></returns>
        DbType MapDbType(PropertyInfo info);
        /// <summary>
        /// Maps the value for <see cref="DbParameter.Value"/> from a <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value for the parameter</param>
        /// <returns>Returns the value for <see cref="DbParameter.Value"/></returns>
        object MapParameterValue(object value);
        /// <summary>
        /// Maps the value for <see cref="DbParameter.Value"/> from a <paramref name="value"/> and an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="value">The value for the parameter</param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns the value for <see cref="DbParameter.Value"/></returns>
        object MapParameterValue(object value, PropertyInfo info);
        /// <summary>
        /// Maps the <see cref="ParameterDirection"/> from an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns a value of <see cref="ParameterDirection"/></returns>
        ParameterDirection MapParameterDirection(PropertyInfo info);
    }
}