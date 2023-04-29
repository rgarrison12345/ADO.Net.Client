using ADO.Net.Client.Core;
using ADO.Net.Client.Implementation;
using System.Data.Common;

namespace ADO.Net.Client
{
    /// <summary>
    /// Represents the base class for all <see cref="DbClient"/> classes
    /// </summary>
    /// <seealso cref="DbProvider"/>
    /// <remarks>
    /// DbClient is a utility class that encompasses both a <see cref="DbConnection"/> and a <see cref="DbCommand"/>
    /// to help query a database with minimal coding to focus on the users SQL
    /// </remarks>
    public partial class DbClient : DbProvider
    {
        private readonly ISqlExecutor _executor;

        /// <summary>
        /// An instance of <see cref="IConnectionManager"/>
        /// </summary>
        public override IConnectionManager ConnectionManager => _executor.ConnectionManager;
        
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with an instance of <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public DbClient(ISqlExecutor executor)
        {
            _executor = executor;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbClient"/> class.
        /// </summary>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        /// <param name="manager">An instance of <see cref="IConnectionManager"/></param>
        public DbClient(IDbObjectFactory factory, IConnectionManager manager) : this (factory, manager, new DataMapper())
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbClient"/> class.
        /// </summary>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        /// <param name="manager">An instance of <see cref="IConnectionManager"/></param>
        /// <param name="mapper">An instance of <see cref="IDataMapper"/></param>
        public DbClient(IDbObjectFactory factory, IConnectionManager manager, IDataMapper mapper) : this(new SqlExecutor(factory, manager, mapper))
        {

        }
    }
}