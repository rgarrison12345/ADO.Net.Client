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
using ADO.Net.Client.Tests.Common.Models;
using NUnit.Framework;
using System.Reflection;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class UtilitiesTest
    {
        #region Tests        
        /// <summary>
        /// Cannots the name of the get property by.
        /// </summary>
        [Test]
        public void CannotGetPropertyByName()
        {
            PropertyInfo[] properties = typeof(DbTypeModel).GetProperties();
            PropertyInfo property = properties.GetProperty("SomePropertyName");

            Assert.IsNull(property);
        }
        /// <summary>
        /// Gets the name of the property by.
        /// </summary>
        [Test]
        public void GetPropertyByName()
        {
            DbTypeModel model = new DbTypeModel();
            PropertyInfo[] properties = typeof(DbTypeModel).GetProperties();

            PropertyInfo property = properties.GetProperty(nameof(model.NormalDateTime));

            Assert.IsNotNull(property);
            Assert.That(property.Name == nameof(model.NormalDateTime));
        }
        #endregion
    }
}