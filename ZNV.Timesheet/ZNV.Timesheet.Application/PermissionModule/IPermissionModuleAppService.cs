using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.PermissionModule
{
    public interface IPermissionModuleAppService : IApplicationService
    {
        List<PermissionModule> GetPermissionModuleList();
        List<PermissionModule> GetPermissionModuleList(int start, int length, string sortColumnName, string sortDirection, out int totalCount);
        PermissionModule GetPermissionModule(int id);
        int AddPermissionModule(PermissionModule permissionModule);
        PermissionModule UpdatePermissionModule(PermissionModule permissionModule);
        void DeleteRole(int id);
    }
}
