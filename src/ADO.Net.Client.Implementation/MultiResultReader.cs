using ADO.Net.Client.Core;
using System.Data.Common;

namespace ADO.Net.Client.Implementation
{
    /// <summary>
    /// Reader object that reads through multiple result sets
    /// </summary>
    /// <seealso cref="IMultiResultReader"/>
    public partial class MultiResultReader : IMultiResultReader
    {
        private bool disposedValue = false; // To detect redundant calls
        private readonly DbDataReader _reader = null;
        private readonly IDataMapper _mapper = null;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiResultReader"/> class.
        /// </summary>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains data to read through</param>
        public MultiResultReader(DbDataReader reader) : this(reader, new DataMapper())
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiResultReader"/> class.
        /// </summary>
        /// <param name="mapper">An instance of <see cref="IDataMapper"/> that will map data contained in the <paramref name="reader"/></param>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains data to read through</param>
        public MultiResultReader(DbDataReader reader, IDataMapper mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }
    }
}