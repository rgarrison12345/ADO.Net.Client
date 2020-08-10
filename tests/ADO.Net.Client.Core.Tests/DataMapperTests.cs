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
using ADO.Net.Client.Tests.Common;
using ADO.Net.Client.Tests.Common.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
#endregion

namespace ADO.Net.Client.Core.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [Category("Mapper Tests")]
    public class DataMapperTests
    {
        #region Constructors
        #endregion
        #region Setup/Teardown
        #endregion
        #region Tests        
        /// <summary>
        /// Maps the record create instance.
        /// </summary>
        [Test]
        public void MapRecordCreateInstance()
        {
            DataMapper mapper = new DataMapper();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {

            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = mapper.MapRecord<Employee>(record);

            Assert.IsNotNull(model);
        }
        /// <summary>
        /// Maps the normal property database null.
        /// </summary>
        [Test]
        public void MapNormalPropertyDBNull()
        {
            DataMapper mapper = new DataMapper();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("PhoneNumber", DBNull.Value)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = mapper.MapRecord<Employee>(record);

            Assert.IsNull(model.PhoneNumber);
        }
        /// <summary>
        /// Maps the normal property.
        /// </summary>
        [Test]
        public void MapNormalProperty()
        {
            DataMapper mapper = new DataMapper();
            Guid employeedID = Guid.NewGuid();
            List<KeyValuePair<string, object>> kvp = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("EmployeeID", employeedID)
            };
            CustomDataRecord record = new CustomDataRecord(kvp);
            Employee model = mapper.MapRecord<Employee>(record);

            Assert.AreEqual(employeedID, model.EmployeeID);
        }
        #endregion
    }
}