using System;

namespace ADO.Net.Client.Annotations
{
    /// <summary>
    /// Indicates if the property is considered to be a foreign key field
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKey : Attribute
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKey"/> class.
        /// </summary>
        public ForeignKey()
        {
        }
        #endregion
    }
}