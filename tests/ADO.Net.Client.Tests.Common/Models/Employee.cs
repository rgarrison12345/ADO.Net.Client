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
#endregion

namespace ADO.Net.Client.Tests.Common.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DbFieldIgnore]
        public decimal IgnoreField { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DepartmentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DbField("Earnings")]
        public decimal Salary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EmployeeType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DbField("EmployeeTitle", "SoftwareDeveloper")]
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RecordCreated { get; set; }
#if NET6_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        public DateOnly HireDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeOnly LunchTime { get; set; }
#endif
        /// <summary>
        /// 
        /// </summary>
        public Guid? ManagerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PhoneType? PhoneType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ParameterName("loginCredential")]
        public string Password { private get; set; }
        /// <summary>
        /// 
        /// </summary>
        [IgnoreParameter]
        public string UserName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum PhoneType
    {
        /// <summary>
        /// 
        /// </summary>
        Home = 1,
        /// <summary>
        /// 
        /// </summary>
        Business = 2,
        /// <summary>
        /// 
        /// </summary>
        Mobile = 3
    }
    /// <summary>
    /// 
    /// </summary>
    public enum EmployeeType
    {
        /// <summary>
        /// 
        /// </summary>
        Employee = 1,
        /// <summary>
        /// 
        /// </summary>
        Manager = 2
    }
}