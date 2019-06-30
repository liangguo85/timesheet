using System;
using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.Employee
{
    public interface IEmployeeAppService : IApplicationService
    {
        List<HREmployee> GetEmployeeList();

        List<HRDepartment> GetDepartmentList();
    }
}
