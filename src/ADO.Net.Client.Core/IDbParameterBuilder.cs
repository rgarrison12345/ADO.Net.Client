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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core
{
    /// <summary>
    /// Contract for creating instances of <see cref="DbParameter"/>
    /// </summary>
    public interface IDbParameterBuilder
    {
        #region Helper Methods
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterValue">The value to assign to the <see cref="DbParameter"/></param>
        /// <param name="info">An instance of <see cref="PropertyInfo"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(object parameterValue, PropertyInfo info);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue);
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/> from the passed in <paramref name="values"/>
        /// </summary>
        /// <param name="values">An array of values to be used to create <see cref="DbParameter"/></param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="DbParameter"/></returns>
        IEnumerable<DbParameter> CreateParameters(params object[] values);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/>
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection = ParameterDirection.Input);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> for a variable length data type
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> for a fixed size parameter
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="scale">The number of decimal places to which the <see cref="DbParameter.Value"/> property is resolved.  The default value is <c>null</c></param>
        /// <param name="precision">The maximum number of digits used to represent the <see cref="DbParameter.Value"/> property.  The default value is <c>null</c></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter CreateParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input);
        #endregion
    }
}