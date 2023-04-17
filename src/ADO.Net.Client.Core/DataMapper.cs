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

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Mapper class that maps fields in a record in a database to a .NET object
    /// </summary>
    /// <seealso cref="IDataMapper" />
    public class DataMapper : IDataMapper
    {
        /// <summary>
        /// <c>true</c> if columns named like User_ID should be mapped to property called UserID, <c>false</c> otherwise
        /// </summary>
        public bool MatchUnderscoreNames { get; }
        
        /// <summary>
        /// Initializes a new instance of <see cref="DataMapper"/>
        /// </summary>
        /// <param name="matchUnderscoreNames"><c>true</c> if columns named like User_ID should be mapped to property called UserID, <c>false</c> otherwise</param>
        public DataMapper(bool matchUnderscoreNames = false)
        {
            MatchUnderscoreNames = matchUnderscoreNames;
        }

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
    }
}