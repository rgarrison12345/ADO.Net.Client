using ADO.Net.Client.Core;
using System.Data.Common;

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Utility class that provides methods for both retrieving and modifying data in a data store
    /// </summary>
    /// <seealso cref="ISqlExecutor"/>
    /// <remarks>
    /// SqlExecutor is a class that encompasses a <see cref="DbCommand"/> but the user of
    /// the class must manage the <see cref="DbConnection"/> the class will use
    /// </remarks>
    public partial class SqlExecutor : ISqlExecutor
    {
        private readonly IDataMapper _mapper;
        private readonly IDbObjectFactory _factory;
        private readonly IConnectionManager _manager;

        /// <summary>
        /// An instance of <see cref="IConnectionManager"/>
        /// </summary>
        public IConnectionManager ConnectionManager => _manager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecutor"/> class.
        /// </summary>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        /// <param name="manager">An instance of <see cref="IConnectionManager"/></param>
        public SqlExecutor(IDbObjectFactory factory, IConnectionManager manager) : this(factory, manager, new DataMapper())
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecutor"/> class.
        /// </summary>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        /// <param name="manager">An instance of <see cref="IConnectionManager"/></param>
        /// <param name="mapper">An instance of <see cref="IDataMapper"/></param>
        public SqlExecutor(IDbObjectFactory factory, IConnectionManager manager, IDataMapper mapper)
        {
            _manager = manager;
            _factory = factory;
            _mapper = mapper;
        }
    }
}