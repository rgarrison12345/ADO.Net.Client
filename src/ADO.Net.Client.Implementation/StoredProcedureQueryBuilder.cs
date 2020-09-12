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
using ADO.Net.Client.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Class for creating stored procedure queries
    /// </summary>
    /// <seealso cref="QueryBuilder"/>
    /// <seealso cref="IStoredProcedureQueryBuilder"/>
    public class StoredProcedureQueryBuilder : QueryBuilder, IStoredProcedureQueryBuilder
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureQueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder">An instance of <see cref="IDbParameterBuilder"/></param>
        public StoredProcedureQueryBuilder(IDbParameterBuilder parameterBuilder) : base(parameterBuilder)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureQueryBuilder"/> class.
        /// </summary>
        /// <param name="paramBuilder">An instance of <see cref="IDbParameterBuilder"/></param>
        /// <param name="parameters">The database parameters associated with a query</param>
        public StoredProcedureQueryBuilder(IDbParameterBuilder paramBuilder, IEnumerable<DbParameter> parameters) : base(paramBuilder, parameters)
        {
        }
        #endregion
        #region Methods
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the existing parameters and passed in <paramref name="storedProcedure"/>
        /// </summary>
        /// <param name="storedProcedure">The name of the stored procedure to execute</param>
        /// <param name="clearContents">If <c>true</c> when building the query the current parameters will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        public ISqlQuery CreateStoredProcedure(string storedProcedure, int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true)
        {
            return CreateSQLQuery(storedProcedure, CommandType.StoredProcedure, commandTimeout, shouldBePrepared, clearContents);
        }
        #endregion
    }
}