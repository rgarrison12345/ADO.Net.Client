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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Transfer object that contains the information about a query to use when querying a datastore
    /// </summary>
    /// <seealso cref="ISqlQuery"/>
    public class SqlQuery : ISqlQuery
    {
        #region Fields/Properties
        /// <summary>
        /// Indicates if the current <see cref="QueryText"/> needs to be prepared (or compiled) version of the command on the data source.
        /// </summary>
        public bool ShouldBePrepared { get; internal set; }
        /// <summary>
        /// Represents how a command should be interpreted by the data provider
        /// </summary>
        public CommandType QueryType { get; private set; }
        /// <summary>
        /// The query command text or name of stored procedure to execute against the data store
        /// </summary>
        public string QueryText { get; private set; }
        /// <summary>
        /// The query database parameters that are associated with a query
        /// </summary>
        public IEnumerable<DbParameter> Parameters { get; private set; }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.  Default is 30 seconds
        /// </summary>
        public int CommandTimeout { get; internal set; } = 30;
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates the SQL Query with text, command type, and parameter list
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        /// <param name="parameters">The <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> that are associated with a query</param>
        internal SqlQuery(string query, CommandType type, IEnumerable<DbParameter> parameters)
        {
            QueryText = query;
            QueryType = type;
            Parameters = parameters;
        }
        #endregion
        #region Utility Methods
        #endregion
    }
}