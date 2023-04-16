using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Indicates if the property is considered to be a unique key field
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueKey : Attribute
    {
    }
}