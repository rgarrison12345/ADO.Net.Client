using System.Collections.Generic;
using System.Data.Common;

namespace ADO.Net.Client.Core.Tests
{
    public class CustomQueryBuilder : QueryBuilder
    {
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
    }
}
