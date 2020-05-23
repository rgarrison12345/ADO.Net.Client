﻿#region Licenses
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
using System;
using System.Collections;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Static utility class for assembly
    /// </summary>
    public static class Utilities
    {
        #region Async Methods
        #endregion
        #region Sync Methods   
        #endregion
        #region Helper Methods            
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