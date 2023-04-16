using System;
using System.Data;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Signifies if the data type is <see cref="DbType.DateTime2"/>
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTime2 : Attribute
    {
    }
}
