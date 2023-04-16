using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Indicates if the property is an auto incremented value field
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrement : Attribute
    {
    }
}