using System;
using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.Employee
{
    public interface IEmployeeAppService : IApplicationService
    {
        List<HREmployee> GetEmployeeList();

        List<HREmployee> GetEmployeeList(string searchTerm, int pageSize, int pageNum, out int totalCount);

        HREmployee GetEmployeeByCode(string employeeCode);

        List<HRDepartment> GetDepartmentList();

        List<HRDepartment> GetDepartmentList(string searchTerm, int pageSize, int pageNum, out int totalCount);

        HRDepartment GetDepartmentByCode(string departmentCode);
    }
}
