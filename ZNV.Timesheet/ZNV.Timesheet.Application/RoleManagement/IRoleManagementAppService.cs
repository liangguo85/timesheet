using System.Collections.Generic;
using Abp.Application.Services;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.RoleManagement
{
    public interface IRoleManagementAppService : IApplicationService
    {
        List<Role> GetRoleList(int start, int length, string sortColumnName, string sortDirection, out int totalCount);
        Role GetRole(int id);
        int AddRole(Role role);
        Role UpdateRole(Role role);
        void DeleteRole(int id);
        List<PermissionModule.PermissionModule> GetRoleModules(int roleId);
        void AddRoleModules(int roleId, List<int> moduleIds, string creator);
        List<HRDepartment> GetRoleDepartments(int roleId);
        void AddRoleDepartments(int roleId, List<string> departmentIds, string creator);
        List<PermissionModule.PermissionModule> GetEmployeeModules(string employeeId);
    }
}
