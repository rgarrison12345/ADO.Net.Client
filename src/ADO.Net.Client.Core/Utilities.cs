#region Licenses
/*MIT License
Copyright(c) 2020
Robert Garrison

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion
#region Using Statements
using ADO.Net.Client.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Static utility class for assembly
    /// </summary>
    public static class Utilities
    {
        #region Helper Methods         
        /// <summary>
        /// Gets an instance of <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="matchUnderscoreNames"><c>true</c> if columns named like User_ID should be mapped to property called UserID, <c>false</c> otherwise</param>
        /// <param name="properties">An instance of <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/></param>
        /// <param name="propertyName">The name of a property as a <see cref="string"/> in the <paramref name="properties"/></param>
        /// <returns>Returns an instance of <see cref="PropertyInfo"/>, null if one cannot be found</returns>
        public static PropertyInfo GetProperty(this IEnumerable<PropertyInfo> properties, string propertyName, bool matchUnderscoreNames = false)
        {
            if (matchUnderscoreNames)
            {
                propertyName = propertyName.Replace("_", "");
            }

            //Get the property if it exists
            return properties.Where(x => string.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase) == true).FirstOrDefault();
        }
        /// <summary>
        /// Gets a signle instance of <see cref="PropertyInfo"/> where the <see cref="DbField.DatabaseFieldName"/> matches the passed in <paramref name="name"/>
        /// </summary>
        /// <param name="name">A property name as a value of <see cref="string"/></param>
        /// <param name="infos">An instance of <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/></param>
        /// <returns>Returns an instance of <see cref="PropertyInfo"/></returns>
        public static PropertyInfo GetPropertyInfoByDbField(this IEnumerable<PropertyInfo> infos, string name)
        {
            return infos.Where(x => x.GetCustomAttributes(false).OfType<DbField>().Any(x => string.Equals(x.DatabaseFieldName, name, StringComparison.OrdinalIgnoreCase) == true)).FirstOrDefault();
        }
        /// <summary>
        /// Determines whether this instance is an <see cref="IEnumerable"/>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is enumerable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEnumerable(this object input)
        {
            return (input as IEnumerable) != null && (input as string) == null && (input as byte[]) == null;
        }
        /// <summary>
        /// Gets the type from value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T GetTypeFromValue<T>(object value)
        {
            // Handle nullable types
            Type u = Nullable.GetUnderlyingType(typeof(T));

            //Check if this is any form of null
            if (u != null && (value == null || value == DBNull.Value))
            {
                return default;
            }

            //Return this back to the caller
            return (T)Convert.ChangeType(value, u ?? typeof(T));
        }
        /// <summary>
        /// Checks if the passed in type is a generic type that is nullable
        /// </summary>
        /// <param name="type">The .NET type to check for nullable</param>
        /// <returns>Returns true if the passed in type is nullable, false otherwise</returns>
        public static bool IsNullableGenericType(this Type type)
        {
            TypeInfo info = type.GetTypeInfo();

            //Return this back to the caller
            return (info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        #endregion
    }
}