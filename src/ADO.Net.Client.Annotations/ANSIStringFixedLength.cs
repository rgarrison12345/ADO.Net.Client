using System;
using System.Data;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Signifies if the data type is <see cref="DbType.AnsiStringFixedLength"/>
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class ANSIStringFixedLength : Attribute
    {
    }
}