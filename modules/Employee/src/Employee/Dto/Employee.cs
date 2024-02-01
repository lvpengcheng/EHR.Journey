using EHR.Journey.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public class Employee : BaseEntity<Employee>,IExtensionData
    {
        public string? Name { get; set; }
        public string? ExtensionData { get; set; }
    }
    public class EmployeeInput : BaseInput
    {
        public string? Name { get; set; }
        public string? ExtensionData { get; set; }
    }
}
