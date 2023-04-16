using System;
using System.Data;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Database parameter direction that represents <see cref="ParameterDirection.ReturnValue"/>
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class ReturnValue : Attribute
    {
    }
}