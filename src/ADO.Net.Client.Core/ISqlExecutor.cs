namespace ADO.Net.Client.Core
{
    /// <summary>
    /// The contract class for a ISqlExecutor based class
    /// </summary>
    /// <seealso cref="ISqlExecutorAsync"/>
    /// <seealso cref="ISqlExecutorSync"/>
    public interface ISqlExecutor : ISqlExecutorSync, ISqlExecutorAsync
    {
        /// <summary>
        /// An instance of <see cref="IConnectionManager"/>
        /// </summary>
        public IConnectionManager ConnectionManager { get; }
    }
}