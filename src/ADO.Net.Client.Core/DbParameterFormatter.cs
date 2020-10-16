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
        public virtual DbType MapDbType(PropertyInfo info)
        {
            //Check if this is a byte array
            if (info.PropertyType.Name == "Byte[]")
            {
                return DbType.Binary;
            }
            else if (info.PropertyType == typeof(DateTime))
            {
                //Check which date type to map to
                if (Attribute.IsDefined(info, typeof(DateTime2), false))
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
                if (HasNativeGuidSupport)
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
                if (Attribute.IsDefined(info, typeof(ANSIString), false))
                {
                    return DbType.AnsiString;
                }
                else if (Attribute.IsDefined(info, typeof(ANSIStringFixedLength), false))
                {
                    return DbType.AnsiStringFixedLength;
                }
                else if (Attribute.IsDefined(info, typeof(StringFixedLength), false))
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
        /// Maps the value for <see cref="DbParameter.Value"/> from a <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value for the parameter</param>
        /// <returns>Returns the value for <see cref="DbParameter.Value"/></returns>
        public virtual object MapParameterValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            
            if (value is Enum)
            {
                return GetEnumValue(value);
            }
            else if (value is Guid && !HasNativeGuidSupport)
            {
                return value.ToString();
            }

            return value;
        }
        /// <summary>
        /// Maps the value for <see cref="DbParameter.Value"/> from a <paramref name="value"/> and an instance of <paramref name="info"/>
        /// </summary>
        /// <param name="value">The value for the parameter</param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns the value for <see cref="DbParameter.Value"/></returns>
        public virtual object MapParameterValue(object value, PropertyInfo info)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else if (info.PropertyType.IsEnum)
            {
                return GetEnumValue(value);
            }
            else if (info.PropertyType == typeof(Guid) && !HasNativeGuidSupport)
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
        public virtual ParameterDirection MapParameterDirection(PropertyInfo info)
        {
            if (Attribute.IsDefined(info, typeof(Input), false))
            {
                return ParameterDirection.Input;
            }
            if (Attribute.IsDefined(info, typeof(Output), false))
            {
                return ParameterDirection.Output;
            }
            if (Attribute.IsDefined(info, typeof(ReturnValue), false))
            {
                return ParameterDirection.ReturnValue;
            }

            return ParameterDirection.InputOutput;
        }
        /// <summary>
        /// Maps a parameter name from an instance of <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns the name of a parameter</returns>
        public virtual string MapParameterName(PropertyInfo info)
        {
            string parameterName = info.Name;

            //Check if there's an alternate parameter name
            if (Attribute.IsDefined(info, typeof(ParameterName)))
            {
                parameterName = ((ParameterName)info.GetCustomAttributes(false).Where(x => x.GetType() == typeof(ParameterName)).Single()).Name;
            }

            return MapParameterName(parameterName);
        }
        /// <summary>
        /// Maps the name of the parameter for <see cref="DbParameter.ParameterName"/>
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>Returns the name of a parameter to be used for <see cref="DbParameter.ParameterName"/></returns>
        public virtual string MapParameterName(string parameterName)
        {
            return parameterName.StartsWith(ParameterNamePrefix) ? parameterName : string.Concat(ParameterNamePrefix, parameterName);
        }
        /// <summary>
        /// Maps an instance of a <see cref="DbParameter"/> using the passed in <paramref name="info"/> <paramref name="parameterValue"/>
        /// </summary>
        /// <param name="parameter">An instance of <see cref="DbParameter"/></param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        public virtual void MapDbParameter(DbParameter parameter, object parameterValue, PropertyInfo info)
        {
            parameter.ParameterName = MapParameterName(info);
            parameter.Value = MapParameterValue(parameterValue, info);
            parameter.DbType = MapDbType(info);
            parameter.Direction = MapParameterDirection(info);

            //Help query plan caching by using common size if this is a string or Guid Type
            if (info.PropertyType == typeof(Guid) && !HasNativeGuidSupport)
            {
                parameter.Size = 40;
            }
            else if (parameter.DbType == DbType.String 
                     || parameter.DbType == DbType.StringFixedLength
                     || parameter.DbType == DbType.AnsiString
                     || parameter.DbType == DbType.AnsiStringFixedLength)
            {
                parameter.Size = Math.Max(parameter.Value.ToString().Length + 1, 4000);
            }
        }
        #endregion
        #region  Helper Methods        
        /// <summary>
        /// Gets the type code.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private TypeCode GetTypeCode(object value)
        {
            Type type = value.GetType();

            if (value is IConvertible convertible)
            {
                return convertible.GetTypeCode();
            }
            if (type.IsEnum)
            {
                return GetEnumTypeCode((Enum)value);
            }

            return Type.GetTypeCode(type);
        }
        /// <summary>
        /// Gets the enum type code.
        /// </summary>
        /// <param name="value">The value as an instance of <see cref="Enum"/></param>
        /// <returns>Returns an instance of <see cref="TypeCode"/></returns>
        private TypeCode GetEnumTypeCode(Enum value)
        {
            return Type.GetTypeCode(Enum.GetUnderlyingType(value.GetType()));
        }
        /// <summary>
        /// Gets the value of an <see cref="Enum"/> based on the <see cref="TypeCode"/>
        /// </summary>
        /// <param name="value">Value as an instance of <see cref="Enum"/></param>
        /// <exception cref="ArgumentException">Thrown when the passed in <paramref name="value"/> is not an <see cref="Enum"/></exception>
        /// <returns>Returns the value derived from the <see cref="Enum"/> <see cref="TypeCode"/></returns>
        private object GetEnumValue(object value)
        {
            if (!value.GetType().IsEnum)
            {
                throw new ArgumentException($"{nameof(value)} is not an enumeration type");
            }

            switch (GetEnumTypeCode((Enum)value))
            {
                case TypeCode.Byte: return (byte)value;
                case TypeCode.SByte: return (sbyte)value;
                case TypeCode.Int16: return (short)value;
                case TypeCode.Int32: return (int)value;
                case TypeCode.Int64: return (long)value;
                case TypeCode.UInt16: return (ushort)value;
                case TypeCode.UInt32: return (uint)value;
                case TypeCode.UInt64: return (ulong)value;
            }

            return value;
        }
        #endregion
    }
}