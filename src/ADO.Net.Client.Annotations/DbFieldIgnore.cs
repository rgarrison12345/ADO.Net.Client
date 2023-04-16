using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Attribute class that defines if a property should be read out of a database query
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldIgnore : Attribute
    {
    }
}