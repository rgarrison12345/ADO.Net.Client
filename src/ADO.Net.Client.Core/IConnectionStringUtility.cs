using System.Collections.Generic;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract to modify/retrieve values in a connection string
    /// </summary>
    public interface IConnectionStringUtility
    {
        /// <summary>
        /// Adds a property name and value to the current connection string
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <param name="value">The value to use with the connection string property</param>
        void AddConnectionStringProperty(string name, object value);
        /// <summary>
        /// Removes a connection string property from the connection string by name
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        void RemoveConnectionStringProperty(string name);
        /// <summary>
        /// Retrieves a connection string property value as an object
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <returns>Returns a connection string property as an <see cref="object"/></returns>
        object GetConnectionStringPropertyValue(string name);
        /// <summary>
        /// Configures the connection string with the key value pairs passed into the routine
        /// </summary>
        /// <param name="properties">Key value pairs of connection string property names and values</param>
        void ConfigureConnectionString(IDictionary<string, object> properties);
        /// <summary>
        /// Clears the contents of the connection string
        /// </summary>
        void ClearConnectionString();
        /// <summary>
        /// Checks if passed in <paramref name="keyword"/> is an allowable keyword in a connection string by the current provider
        /// </summary>
        /// <param name="keyword">The keyword to check in the providers allowable connection string keywords</param>
        /// <returns>Returns a <see cref="bool"/> indicating if the providers connection string allows the passed in keyword</returns>
        bool ConnectionStringAllowsKey(string keyword);
    }
}
