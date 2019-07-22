using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet.Employee;
using System.Linq.Dynamic;

namespace ZNV.Timesheet.PermissionModule
{
    public class PermissionModuleAppService : TimesheetAppServiceBase, IPermissionModuleAppService
    {
        private readonly IPermissionModuleRepository _permissionModuleRepository;

        public PermissionModuleAppService(IPermissionModuleRepository permissionModuleRepository)
        {
            _permissionModuleRepository = permissionModuleRepository;
        }
        public List<PermissionModule> GetPermissionModuleList()
        {
            return _permissionModuleRepository.GetAll().OrderBy(item => item.ModuleCode).ToList();
        }
        public List<PermissionModule> GetPermissionModuleList(int start, int length, string sortColumnName, string sortDirection, out int totalCount)
        {
            totalCount = _permissionModuleRepository.Count();
            return _permissionModuleRepository.GetAll().OrderBy(sortColumnName + " " + sortDirection).Skip(start).Take(length).ToList();
        }
        public PermissionModule GetPermissionModule(int id)
        {
            return _permissionModuleRepository.GetAll().Where(item => item.Id == id).FirstOrDefault();
        }
        public int AddPermissionModule(PermissionModule permissionModule) {
            if (permissionModule.ParentModuleId != null)
            {
                permissionModule.Level = GetPermissionModule(permissionModule.ParentModuleId.Value).Level + 1;
            }
            return _permissionModuleRepository.InsertAndGetId(permissionModule);
        }
        public PermissionModule UpdatePermissionModule(PermissionModule permissionModule) {
            return _permissionModuleRepository.Update(permissionModule);
        }
        public void DeleteRole(int id) {
            _permissionModuleRepository.Delete(id);
        }
    }
}
