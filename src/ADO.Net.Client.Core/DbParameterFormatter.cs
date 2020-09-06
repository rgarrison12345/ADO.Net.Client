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
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// A <see cref="DbParameter"/> formatter class that implements <see cref="IDbParameterFormatter"/>
    /// </summary>
    /// <seealso cref="IDbParameterFormatter" />
    public class DbParameterFormatter : IDbParameterFormatter
    {
        #region Fields/Properties        
        /// <summary>
        /// Gets a value indicating whether this instance has native unique identifier support.  Defaults to <c>true</c>
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has native unique identifier support; otherwise, <c>false</c>.
        /// </value>
        public bool HasNativeGuidSupport { get; private set; } = true;
        /// <summary>
        /// Gets or sets the parameter name prefix.  Defaults to @
        /// </summary>
        /// <value>
        /// The parameter name prefix.
        /// </value>
        public string ParameterNamePrefix { get; private set; } = "@";
        #endregion
        #region Constructors                
        /// <summary>
        /// Initializes a new instance of the <see cref="DbParameterFormatter"/> class.
        /// </summary>
        /// <param name="nativeGuidSupport">if set to <c>true</c> [native unique identifier support].</param>
        /// <param name="parameterNamePrefix">The parameter name prefix.</param>
        public DbParameterFormatter(bool nativeGuidSupport = false, string parameterNamePrefix = "@")
        {
            HasNativeGuidSupport = nativeGuidSupport;
            ParameterNamePrefix = parameterNamePrefix;
        }
        #endregion
        #region Utility Methods                
        /// <summary>
        /// Maps the type value of a <see cref="DbType"/> from an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns a value of <see cref="DbType"/></returns>
        public DbType MapDbType(PropertyInfo info)
        {
            //Check if this is a byte array
            if (info.PropertyType.Name == "Byte[]")
            {
                return DbType.Binary;
            }
            else if (info.PropertyType == typeof(DateTime))
            {
                //Check which date type to map to
                if (Attribute.IsDefined(info, typeof(DateTime2), false) == true)
                {
                    return DbType.DateTime2;
                }
                else
                {
                    return DbType.DateTime;
                }
            }
            else if (info.PropertyType == typeof(DateTimeOffset))
            {
                return DbType.DateTimeOffset;
            }
            else if (info.PropertyType == typeof(TimeSpan))
            {
                return DbType.Time;
            }
            else if (info.PropertyType == typeof(float))
            {
                return DbType.Single;
            }
            else if (info.PropertyType == typeof(bool))
            {
                return DbType.Boolean;
            }
            else if (info.PropertyType == typeof(sbyte))
            {
                return DbType.SByte;
            }
            else if (info.PropertyType == typeof(byte))
            {
                return DbType.Byte;
            }
            else if (info.PropertyType == typeof(double))
            {
                return DbType.Double;
            }
            else if (info.PropertyType == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (info.PropertyType == typeof(short))
            {
                return DbType.Int16;
            }
            else if (info.PropertyType == typeof(int))
            {
                return DbType.Int32;
            }
            else if (info.PropertyType == typeof(long))
            {
                return DbType.Int64;
            }
            else if (info.PropertyType == typeof(ushort))
            {
                return DbType.UInt16;
            }
            else if (info.PropertyType == typeof(uint))
            {
                return DbType.UInt32;
            }
            else if (info.PropertyType == typeof(ulong))
            {
                return DbType.UInt64;
            }
            else if (info.PropertyType == typeof(Guid))
            {
                if (HasNativeGuidSupport == true)
                {
                    return DbType.Guid;
                }
                else
                {
                    return DbType.String;
                }
            }
            else if (info.PropertyType == typeof(string))
            {
                //Check which string type to map to
                if (Attribute.IsDefined(info, typeof(ANSIString), false) == true)
                {
                    return DbType.AnsiString;
                }
                else if (Attribute.IsDefined(info, typeof(ANSIStringFixedLength), false) == true)
                {
                    return DbType.AnsiStringFixedLength;
                }
                else if (Attribute.IsDefined(info, typeof(StringFixedLength), false) == true)
                {
                    return DbType.StringFixedLength;
                }
                else
                {
                    return DbType.String;
                }
            }
            else
            {
                return DbType.Object;
            }
        }
        /// <summary>
        /// Maps the parameter value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object MapParameterValue(object value)
        {
            return value ?? DBNull.Value;
        }
        /// <summary>
        /// Maps the value for <see cref="DbParameter.Value"/> from a <paramref name="value"/> and an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="value">The value for the parameter</param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns the value for <see cref="DbParameter.Value"/></returns>
        public object MapParameterValue(object value, PropertyInfo info)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else if (info.PropertyType.IsEnum == true)
            {
                return Convert.ChangeType(value, ((Enum)value).GetTypeCode());
            }
            else if (info.PropertyType == typeof(Guid) && HasNativeGuidSupport == false)
            {
                return value.ToString();
            }

            return value;
        }
        /// <summary>
        /// Maps the <see cref="ParameterDirection"/> from an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns a value of <see cref="ParameterDirection"/></returns>
        public ParameterDirection MapParameterDirection(PropertyInfo info)
        {
            object[] custom = info.GetCustomAttributes(false);

            if(custom.Where(x => x.GetType() == typeof(Input)).Count() == 1)
            {
                return ParameterDirection.Input;
            }
            else if(custom.Where(x => x.GetType() == typeof(Output)).Count() == 1)
            {
                return ParameterDirection.Output;
            }
            else if (custom.Where(x => x.GetType() == typeof(ReturnValue)).Count() == 1)
            {
                return ParameterDirection.ReturnValue;
            }
            else
            {
                return ParameterDirection.InputOutput;
            }
        }
        /// <summary>
        /// Maps the name of the parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>Returns the name of a parameter</returns>
        public string MapParameterName(string parameterName)
        {
            return parameterName.StartsWith(ParameterNamePrefix) == true ? parameterName : string.Concat(ParameterNamePrefix, parameterName);
        }
        /// <summary>
        /// Maps an instance of a <see cref="IDbDataParameter"/> using the passed in <paramref name="info"/> <paramref name="parameterValue"/>
        /// </summary>
        /// <param name="parameter">An instance of <see cref="IDbDataParameter"/></param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        public void MapDbParameter(DbParameter parameter, object parameterValue, PropertyInfo info)
        {
            parameter.ParameterName = MapParameterName(info.Name);
            parameter.Value = MapParameterValue(parameterValue, info);
            parameter.DbType = MapDbType(info);
            parameter.Direction = MapParameterDirection(info);

            //Help query plan caching by using common size if this is a string or Guid Type
            if (info == typeof(Guid) && HasNativeGuidSupport == false)
            {
                parameter.Size = 40;
            }
            else if (parameter.DbType == DbType.String || parameter.DbType == DbType.StringFixedLength
                || parameter.DbType == DbType.AnsiString || parameter.DbType == DbType.AnsiStringFixedLength)
            {
                parameter.Size = Math.Max(parameter.Value.ToString().Length + 1, 4000);
            }
        }
        #endregion
    }
}