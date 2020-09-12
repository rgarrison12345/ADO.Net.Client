#region Using Statements
using System.Collections.Generic;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    public class CustomQueryBuilder : QueryBuilder
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocQueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder"></param>
        public CustomQueryBuilder(IDbParameterBuilder parameterBuilder) : base(parameterBuilder)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocQueryBuilder"/> class.
        /// </summary>
        /// <param name="parameterBuilder"></param>
        /// <param name="parameters">The database parameters associated with a query</param>
        public CustomQueryBuilder(IDbParameterBuilder parameterBuilder, IEnumerable<DbParameter> parameters) : base(parameterBuilder, parameters)
        {
        }
        #endregion
    }
}
