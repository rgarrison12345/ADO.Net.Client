namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for creating stored procedure queries
    /// </summary>
    public interface IStoredProcedureQueryBuilder
    {
        /// <summary>
        /// Create an instance of <see cref="ISqlQuery"/> using the existing parameters and <paramref name="storedProcedure"/>
        /// </summary>
        /// <param name="storedProcedure">The name of the stored procedure to execute</param>
        /// <param name="clearContents">If <c>true</c> when building the query the current parameters will be cleared</param>
        /// <param name="shouldBePrepared">Indicates if the current sql string needs to be prepared (or compiled) version of the command on the data source.</param>
        /// <param name="commandTimeout">The wait time in seconds before terminating the attempt to execute a command and generating an error</param>
        ISqlQuery CreateStoredProcedure(string storedProcedure, int commandTimeout = 30, bool shouldBePrepared = false, bool clearContents = true);
    }
}