#region Using Statements
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ISqlQuery"/>
    public interface ICacheableQuery : ISqlQuery
    {
        string CacheKey { get; set; }
    }
}
