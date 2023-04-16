using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract that defines how a class that can execute sql queries should return xml from the database
    /// </summary>
    public interface IXMLExecutor
    {
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        XmlReader ExecuteXMLReader(string query);
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        Task<XmlReader> ExecuteXMLReaderAsync(string query, CancellationToken token = default);
    }
}
