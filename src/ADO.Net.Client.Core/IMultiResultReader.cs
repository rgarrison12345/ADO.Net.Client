namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for an instance that reads records from a database result set
    /// </summary>
    /// <see cref="IMultiResultReaderAsync"/>
    /// <see cref="IMultiResultReaderSync"/>
    public interface IMultiResultReader : IMultiResultReaderSync, IMultiResultReaderAsync
    {

    }
}