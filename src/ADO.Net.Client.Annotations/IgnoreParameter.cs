using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Signifies if a property should not be considered a parameter for a database query
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreParameter : Attribute
    {
    }
}