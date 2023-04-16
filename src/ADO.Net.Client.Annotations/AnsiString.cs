using System;
using System.Data;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Signifies if the data type is <see cref="DbType.AnsiString"/> of varying length
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ANSIString : Attribute
    {
    }
}