using System.Collections.Generic;

namespace ZNV.Timesheet.Web.Models
{
    public class RolePermissionModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public List<int> PermissionModuleIds { get; set; }

        public List<PermissionModule.PermissionModule> PermissionModules { get; set; }

        public List<PermissionModule.PermissionModule> AllPermissionModules { get; set; }
    }
}