using System.Collections.Generic;
using System.Data.Common;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract that builds out queries to be executed against a database
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// The database parameters associated with a query
        /// </summary>
        /// <value>
        /// The parameters associated with a query as a <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/>
        /// </value>
        IEnumerable<DbParameter> Parameters { get; }
        
        /// <summary>
        /// Retrieves a <see cref="DbParameter"/> object by using the passed in parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>The specified <see cref="DbParameter"/> by name</returns>
        DbParameter GetParameter(string parameterName);
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="param">An instance of the <see cref="DbParameter"/> object, that is created the by the caller</param>
        /// <returns>Returns a <see cref="DbParameter"/></returns>
        void AddParameter(DbParameter param);
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as an <see cref="object"/></param>
        void AddParameter(string parameterName, object parameterValue);
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="parameters">The parameters that are associated with a database query</param>
        void AddParameterRange(params object[] parameters);
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successfully removed, false otherwise if item was not found in the list</returns>
        bool RemoveParameter(string parameterName);
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> with an existing <see cref="DbParameter.ParameterName"/>
        /// </summary>
        /// <param name="parameterName">The index as a <c>string</c> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        void ReplaceParameter(string parameterName, DbParameter param);
        /// <summary>
        /// Clears all parameters from the parameters collection
        /// </summary>
        void ClearParameters();
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="parameterName"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        void SetParameterValue(string parameterName, object value);
        /// <summary>
        /// Adds an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> objects to the helpers underlying db parameter collection
        /// </summary>
        /// <param name="dbParams">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> to add to the underlying db parameter collection for the connection</param>
        void AddParameterRange(IEnumerable<DbParameter> dbParams);
        /// <summary>
        /// Checks for a parameter in the parameters collection with the passed in name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use when searching the <see cref="Parameters"/></param>
        /// <returns>True if this parameter exists in the parameters collection, false otherwise</returns>
        bool Contains(string parameterName);
        /// <summary>
        /// Determines whether this instance contains the passed in <paramref name="parameter"/>
        /// </summary>
        /// <param name="parameter">An instance of <see cref="DbParameter"/> that may be associated with this instance</param>
        /// <returns>
        ///   <c>true</c> if this instance contains the passed in <paramref name="parameter"/> otherwise, <c>false</c>.
        /// </returns>
        bool Contains(DbParameter parameter);
    }
}