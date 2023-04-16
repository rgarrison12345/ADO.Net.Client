using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Attribute class that defines a field that goes into and comes out of a database
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbField : Attribute
    {
        /// <summary>
        /// The default value as a <see cref="object"/> in the instance where a value from the database is <see cref="DBNull.Value"/>
        /// </summary>
        public object DefaultValueIfNull { get; private set; } = null;
        /// <summary>
        /// The name of a field that is being pulled from a query
        /// </summary>
        public string DatabaseFieldName { get; private set; } = "";
       
        /// <summary>
        /// Initializes a new instance of <see cref="DbField"/>
        /// </summary>
        /// <param name="dbFieldName">The name of a field that exists in a database table</param>
        /// <param name="valueIfNull">The default value if the field coming from a query has a value of <see cref="DBNull.Value"/></param>
        public DbField(string dbFieldName, object valueIfNull = null)
        {
            DefaultValueIfNull = valueIfNull;
            DatabaseFieldName = dbFieldName;
        }
    }
}