namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for all DbProvider based classes
    /// </summary>
    /// <seealso cref="ISynchronousClient"/>
    /// <seealso cref="IAsynchronousClient"/>
    public interface IDbProvider : ISynchronousClient, IAsynchronousClient
    {
        #region Fields/Properties
        /// <summary>
        /// An instance of <see cref="IConnectionManager"/>
        /// </summary>
        public IConnectionManager ConnectionManager { get; }
        #endregion
    }
}