using System;
using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.Employee
{
    public interface IEmployeeAppService : IApplicationService
    {
        List<HREmployee> GetEmployeeList();

        HREmployee GetEmployeeByCode(string employeeCode);

        List<HRDepartment> GetDepartmentList();

        HRDepartment GetDepartmentByCode(string departmentCode);
    }
}
