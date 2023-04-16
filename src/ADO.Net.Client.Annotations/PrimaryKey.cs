using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Indicates if the property is considered to be a primary key field
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    { 
    }
}