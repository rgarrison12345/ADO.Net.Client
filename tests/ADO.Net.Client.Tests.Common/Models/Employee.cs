using ADO.Net.Client.Annotations;
using System;

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