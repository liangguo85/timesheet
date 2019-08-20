using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.Web.Models
{
    public class RoleDepartmentModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public List<string> DepartmentIds { get; set; }

        public List<HRDepartment> Departments { get; set; }
    }
}