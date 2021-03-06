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
using ADO.Net.Client.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
#if !NET45
using System.Runtime.CompilerServices;
#endif
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Mapper class that maps fields in a record in a database to a .NET object
    /// </summary>
    /// <seealso cref="IDataMapper" />
    public class DataMapper : IDataMapper
    {
        #region Fields/Properties
        /// <summary>
        /// <c>true</c> if columns named like User_ID should be mapped to property called UserID, <c>false</c> otherwise
        /// </summary>
        public bool MatchUnderscoreNames { get; }
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="DataMapper"/>
        /// </summary>
        /// <param name="matchUnderscoreNames"><c>true</c> if columns named like User_ID should be mapped to property called UserID, <c>false</c> otherwise</param>
        public DataMapper(bool matchUnderscoreNames = false)
        {
            MatchUnderscoreNames = matchUnderscoreNames;
        }
        #endregion
        #region Mapper Methods
#if !NET40 && !NET45
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server asynchronously
        /// </summary>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public virtual async IAsyncEnumerable<T> MapResultSetStreamAsync<T>(DbDataReader reader, [EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            //Keep looping through the result set
            while (await reader.ReadAsync(token).ConfigureAwait(false))
            {
                //Return this object
                yield return MapRecord<T>(reader);
            }
        }
#endif
#if !NET40
        /// <summary>
        /// Maps an entire result set in the <paramref name="reader"/>
        /// </summary>
        /// <param name="token">Structure that propagates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public virtual async Task<IEnumerable<T>> MapResultSetAsync<T>(DbDataReader reader, CancellationToken token = default) where T : class
        {
            List<T> returnList = new List<T>();

            //Keep looping through the result set
            while (await reader.ReadAsync(token).ConfigureAwait(false))
            {
                //Return this object
                returnList.Add(MapRecord<T>(reader));
            }

            return returnList;
        }
#endif
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public virtual IEnumerable<T> MapResultSetStream<T>(DbDataReader reader) where T : class
        {
            //Keep looping through the result set
            while (reader.Read())
            {
                //Return this object
                yield return MapRecord<T>(reader);
            }
        }
        /// <summary>
        /// Maps an entire result set in the <paramref name="reader"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public virtual IEnumerable<T> MapResultSet<T>(DbDataReader reader) where T : class
        {
            List<T> returnList = new List<T>();

            //Keep looping through the result set
            while (reader.Read())
            {
                //Return this object
                returnList.Add(MapRecord<T>(reader));
            }

            return returnList;
        }
        /// <summary>
        /// Maps the passed in <paramref name="record"/> to an instance of the <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="record"/></typeparam>
        /// <param name="record">An instance of <see cref="IDataRecord"/> to read data from</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="record"/></returns>
        public virtual T MapRecord<T>(IDataRecord record) where T : class
        {
            //Get an instance of the object passed in
            T returnType = Activator.CreateInstance<T>();
            IEnumerable<PropertyInfo> writeableProperties = returnType.GetType().GetProperties().Where(x => x.CanWrite)
                .Where(x => !Attribute.IsDefined(x, typeof(DbFieldIgnore), false));

            //Loop through all fields in this record
            for (int i = 0; i < record.FieldCount; i++)
            {
                string name = record.GetName(i);
                PropertyInfo info = Utilities.FindProperty(writeableProperties, name, MatchUnderscoreNames);

                //Check if a property could be found by name
                if (info == null)
                {
                    continue;
                }

                object value = null;

                //Check if this is the database representation of null
                if (!record.IsDBNull(i))
                {
                    value = record.GetValue(i);
                }

                bool nullable = info.PropertyType.IsNullableGenericType();
                bool isEnum = (nullable == false) ? info.PropertyType.IsEnum : (Nullable.GetUnderlyingType(info.PropertyType)?.IsEnum == true);

                //Check for a dbfield attribute
                if (Attribute.IsDefined(info, typeof(DbField), false) && value == null)
                {
                    //Set new value
                    value = ((DbField)info.GetCustomAttributes(false).Where(x => x.GetType() == typeof(DbField)).Single()).DefaultValueIfNull;
                }

                //Now attempt to set property value
                if (nullable && !isEnum)
                {
                    if (value != null)
                    {
                        info.SetValue(returnType, Convert.ChangeType(value, Nullable.GetUnderlyingType(info.PropertyType)), null);
                    }
                }
                else if (nullable && isEnum)
                {
                    if (value != null)
                    {
                        //Property is an enum
                        info.SetValue(returnType, Enum.Parse(Nullable.GetUnderlyingType(info.PropertyType), value.ToString()), null);
                    }
                }
                else if (isEnum)
                {
                    //Property is an enum
                    info.SetValue(returnType, Enum.Parse(info.PropertyType, value.ToString()), null);
                }
                else
                {
                    //This is a normal property
                    info.SetValue(returnType, Convert.ChangeType(value, info.PropertyType), null);
                }
            }

            //Return this back to the caller
            return returnType;
        }
        #endregion
        #region Helper Methods
        #endregion
    }
}