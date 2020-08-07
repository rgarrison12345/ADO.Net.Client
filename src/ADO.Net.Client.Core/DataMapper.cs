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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
#if !NET45
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public async IAsyncEnumerable<T> MapResultSetStreamAsync<T>(DbDataReader reader, [EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            //Keep looping through the result set
            while (await reader.ReadAsync(token).ConfigureAwait(false) == true)
            {
                //Return this object
                yield return MapRecord<T>(reader);
            }

            yield break;
        }
#endif
        /// <summary>
        /// Maps an entire result set in the <paramref name="reader"/>
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public async Task<IEnumerable<T>> MapResultSetAsync<T>(DbDataReader reader, CancellationToken token = default) where T : class
        {
            List<T> returnList = new List<T>();

            //Keep looping through the result set
            while (await reader.ReadAsync(token).ConfigureAwait(false) == true)
            {
                //Return this object
                returnList.Add(MapRecord<T>(reader));
            }

            return returnList;
        }
        /// <summary>
        /// Maps the result set one at a time by streaming the result from the server
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public IEnumerable<T> MapResultSetStream<T>(DbDataReader reader) where T : class
        {
            //Keep looping through the result set
            while (reader.Read() == true)
            {
                //Return this object
                yield return MapRecord<T>(reader);
            }

            yield break;
        }
        /// <summary>
        /// Maps an entire result set in the <paramref name="reader"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> the caller wants created from the passed in <paramref name="reader"/></typeparam>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that contains a result set of records that needs to be mapped</param>
        /// <returns>Returns the instance of <typeparamref name="T"/> created from the passed in <paramref name="reader"/></returns>
        public IEnumerable<T> MapResultSet<T>(DbDataReader reader) where T : class
        {
            List<T> returnList = new List<T>();

            //Keep looping through the result set
            while (reader.Read() == true)
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
        public T MapRecord<T>(IDataRecord record) where T : class
        {
            //Get an instance of the object passed in
            T returnType = Activator.CreateInstance<T>();
            IEnumerable<PropertyInfo> writeableProperties = returnType.GetType().GetProperties().Where(x => x.CanWrite == true).Where(x => Attribute.IsDefined(x, typeof(DbFieldIgnore), false) == false);

            //Loop through all records in this record
            for (int i = 0; i < record.FieldCount; i++)
            {
                string name = record.GetName(i);
                object value = record.GetValue(i);
                PropertyInfo info = writeableProperties.GetProperty(name) ?? GetPropertyInfoByDbField(name, writeableProperties);

                //Check if a property could be found by name
                if(info == null)
                {
                    continue;
                }

                //Check if this is the databae representation of null
                if (value == DBNull.Value)
                {
                    value = null;
                }

                //Might need to change the value
                if (info.GetCustomAttributes(false).Where(x => x.GetType() == typeof(DbField)).SingleOrDefault() is DbField field && value == null)
                {
                    //Set new value
                    value = field.DefaultValueIfNull;
                }
                if (info.PropertyType.IsNullableGenericType() == true)
                {
                    if (value == null)
                    {
                        info.SetValue(returnType, null, null);
                    }
                    else
                    {
                        info.SetValue(returnType, Convert.ChangeType(value, Nullable.GetUnderlyingType(info.PropertyType)), null);
                    }
                }
                else if (info.PropertyType.GetTypeInfo().IsEnum == true && value != null)
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
        #region Helper Methods
        /// <summary>
        /// Gets a signle instance of <see cref="PropertyInfo"/> where the <see cref="DbField.DatabaseFieldName"/> matches the passed in <paramref name="name"/>
        /// </summary>
        /// <param name="name">A property name as a value of <see cref="string"/></param>
        /// <param name="infos">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns an instance of <see cref="PropertyInfo"/></returns>
        private PropertyInfo GetPropertyInfoByDbField(string name, IEnumerable<PropertyInfo> infos)
        {
            return infos.Where(x => x.GetCustomAttributes(false).OfType<DbField>().Any(x => x.DatabaseFieldName == name)).FirstOrDefault();
        }
        #endregion
    }
}