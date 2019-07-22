using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.RoleManagement
{
    public interface IRoleManagementAppService : IApplicationService
    {
        List<Role> GetRoleList(int start, int length, string sortColumnName, string sortDirection, out int totalCount);
        Role GetRole(int id);
        int AddRole(Role role);
        Role UpdateRole(Role role);
        void DeleteRole(int id);
    }
}
