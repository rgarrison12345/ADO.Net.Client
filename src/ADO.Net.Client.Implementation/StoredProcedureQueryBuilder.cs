using ADO.Net.Client.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Class for creating stored procedure queries
    /// </summary>
    /// <seealso cref="QueryBuilder"/>
    /// <seealso cref="IStoredProcedureQueryBuilder"/>
    public class StoredProcedureQueryBuilder : QueryBuilder, IStoredProcedureQueryBuilder
    {
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
    }
}