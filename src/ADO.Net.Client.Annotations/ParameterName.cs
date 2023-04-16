using System;
using System.Reflection;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Specifies an alternate name of an instance of <see cref="PropertyInfo"/>
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterName : Attribute
    {
        /// <summary>
        /// An alternate name of an instance of <see cref="PropertyInfo"/>
        /// </summary>
        public string Name { get; private set; } = null;

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterName"/>
        /// </summary>
        /// <param name="name">An alternate name of an instance of <see cref="PropertyInfo"/></param>
        public ParameterName(string name)
        {
            Name = name;
        }
    }
}