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
using System.Data;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Factory class for creating instances of <see cref="ISqlQuery"/>
    /// </summary>
    public static class QueryFactory
    {
        #region Methods
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the <paramref name="parameters"/> and <paramref name="storedProcedureName"/> for a stored procedure
        /// </summary>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> assocatiated with a query</param>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        public static ISqlQuery CreateStoredProcedureQuery(string storedProcedureName, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return CreateSQLQuery(storedProcedureName, CommandType.StoredProcedure, parameters, commandTimeout, shouldBePrepared);
        }
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the <paramref name="parameters"/> and built sql query
        /// </summary>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> assocatiated with a query</param>
        /// <param name="queryText">The Ad-Hoc query</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        public static ISqlQuery CreateAdHocQuery(string queryText, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return CreateSQLQuery(queryText, CommandType.Text, parameters, commandTimeout, shouldBePrepared);
        }
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the <paramref name="parameters"/> and built sql query
        /// </summary>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> assocatiated with a query</param>
        /// <param name="queryText">The Ad-Hoc query or stored procedure name</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        public static ISqlQuery CreateSQLQuery(string queryText, CommandType type, IEnumerable<DbParameter> parameters = null, int commandTimeout = 30, bool shouldBePrepared = false)
        {
            return new SqlQuery(queryText, type, parameters) { CommandTimeout = commandTimeout, ShouldBePrepared = shouldBePrepared };
        }
        #endregion
    }
}