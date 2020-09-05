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
#region Using Statements
using System.Collections.Generic;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for creating an Ad-hoc query
    /// </summary>
    public interface IAdHocQueryBuilder
    {
        #region Fields/Properties
        /// <summary>
        /// The query command text to execute against the data store
        /// </summary>
        /// <value>
        /// The <see cref="string"/> value of ad-hoc query
        /// </value>
        string QueryText { get; }
        #endregion
        #region SQL Methods        
        /// <summary>
        /// Clears the underlying SQL query being created by this instance
        /// </summary>
        void ClearSQL();
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        void Append(string sql);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameter">The database parameter associated with this SQL statement</param>
        void Append(string sql, DbParameter parameter);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="paramerterName">Name of the paramerter.</param>
        /// <param name="parmaeterValue">The parmaeter value.</param>
        void Append(string sql, string paramerterName, object parmaeterValue);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built.
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameters">The database parameters associated with this query</param>
        void Append(string sql, params object[] parameters);
        /// <summary>
        /// Appends the specified SQL to the existing SQL statement being built
        /// </summary>
        /// <param name="sql">The SQL statement to append</param>
        /// <param name="parameters">The database parameters associated with this query</param>
        void Append(string sql, IEnumerable<DbParameter> parameters);
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the existing parameters and built sql query
        /// </summary>
        /// <param name="clearContents">If <c>true</c> when building the query the current parameters and <see cref="QueryText"/> will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        ISqlQuery CreateAdHocQuery(int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true);
        #endregion
    }
}
