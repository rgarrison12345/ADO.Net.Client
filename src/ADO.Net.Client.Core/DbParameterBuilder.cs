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
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Class that builds out instances of <see cref="DbParameter"/>
    /// </summary>
    /// <seealso cref="IDbParameterBuilder"/>
    public class DbParameterBuilder : IDbParameterBuilder
    {
        #region Fields/Properties
        private readonly IDbParameterFormatter _formatter;
        private readonly IDbObjectFactory _factory;
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates a new instance of <see cref="DbParameterBuilder"/>
        /// </summary>
        /// <param name="factory">An instance of <see cref="DbProviderFactory"/></param>
        public DbParameterBuilder(DbProviderFactory factory) : this(new DbObjectFactory(factory), new DbParameterFormatter())
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbParameterBuilder"/>
        /// </summary>
        /// <param name="factory">An instance of <see cref="DbProviderFactory"/></param>
        /// <param name="formatter">An instance of <see cref="IDbParameterFormatter"/></param>
        public DbParameterBuilder(DbProviderFactory factory, IDbParameterFormatter formatter) : this(new DbObjectFactory(factory), formatter)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbParameterBuilder"/>
        /// </summary>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        /// <param name="formatter">An instance of <see cref="IDbParameterFormatter"/></param>
        public DbParameterBuilder(IDbObjectFactory factory, IDbParameterFormatter formatter)
        {
            _factory = factory;
            _formatter = formatter;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterValue">The value to assign to the <see cref="DbParameter"/></param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <exception cref="InvalidOperationException">Thrown when the passed in instance of <paramref name="info"/> is an ignored property</exception>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter CreateParameter(object parameterValue, PropertyInfo info)
        {
            //Check if we should create a parameter
            if (Attribute.IsDefined(info, typeof(IgnoreParameter)))
            {
                throw new InvalidOperationException("Cannot create database parameter from this property, it is an ignored property");
            }

            DbParameter parameter = _factory.GetDbParameter();

            _formatter.MapDbParameter(parameter, parameterValue, info);

            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter CreateParameter(string parameterName)
        {
            DbParameter parameter = _factory.GetDbParameter();

            parameter.ParameterName = _formatter.MapParameterName(parameterName);

            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter CreateParameter(string parameterName, object parameterValue)
        {
            //Get the DbParameter object
            DbParameter parameter = CreateParameter(parameterName);

            parameter.Value = _formatter.MapParameterValue(parameterValue);

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = CreateParameter(parameterName, parameterValue);

            //Set parameter properties
            parameter.DbType = dataType;
            parameter.Direction = paramDirection;

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> for a variable length data type
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = CreateParameter(parameterName, parameterValue, dataType, paramDirection);

            //Check for value
            if (size.HasValue)
            {
                parameter.Size = size.Value;
            }

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> from the passed in <paramref name="values"/>
        /// </summary>
        /// <param name="values">An array of values to be used to create <see cref="DbParameter"/></param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/></returns>
        public IEnumerable<DbParameter> CreateParameters(params object[] values)
        {
            List<DbParameter> result = new List<DbParameter>();

            void ProcessArg(object value)
            {
                //Check if this is an enumerable object 
                if (value.IsEnumerable())
                {
                    //Go through each item in the enumerable
                    foreach (var singleArg in value as IEnumerable)
                    {
                        //Recurse the call
                        ProcessArg(singleArg);
                    }
                }
                else if (value is DbParameter parameter)
                {
                    result.Add(parameter);
                }
                else
                {
                    Type type = value.GetType();

                    //Can't have plain string or value types
                    if (type.IsValueType || type == typeof(string))
                    {
                        throw new ArgumentException($"Value type or string passed as Parameter object: {value}");
                    }

                    //We only want properties where we can read a value, and are not an ignored property
                    IEnumerable<PropertyInfo> properties = type.GetProperties().Where(p => p.CanRead == true)
                    .Where(x => Attribute.IsDefined(x, typeof(IgnoreParameter)) == false);

                    //Loop through each property
                    foreach (PropertyInfo prop in properties)
                    {
                        result.Add(CreateParameter(prop.GetValue(value, null), prop));
                    }
                }
            }

            //Go through all items in the params array
            foreach (object arg in values)
            {
                ProcessArg(arg);
            }

            //Nothing to return here
            return result;
        }
#if !NET45
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider for a fixed size parameter
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="scale">The number of decimal places to which the <see cref="DbParameter.Value"/> property is resolved.  The default value is <c>null</c></param>
        /// <param name="precision">The maximum number of digits used to represent the <see cref="DbParameter.Value"/> property.  The default value is <c>null</c></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = CreateParameter(parameterName, parameterValue, dataType, paramDirection);

            //Check for values
            if (precision.HasValue)
            {
                parameter.Precision = precision.Value;
            }
            if (scale.HasValue)
            {
                parameter.Scale = scale.Value;
            }

            //Return this back to the caller
            return parameter;
        }
#endif
        #endregion
    }
}