using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;

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
            var list = (
                         from module in _permissionModuleRepository.GetAll()
                         join parentModule in _permissionModuleRepository.GetAll() on module.ParentModuleId equals parentModule.Id into gj
                         from parentItem in gj.DefaultIfEmpty()
                         select new
                         {
                             module.Id,
                             module.ModuleCode,
                             module.ModuleName,
                             module.ParentModuleId,
                             ParentModuleCode = parentItem.ModuleCode ?? string.Empty,
                             ParentModuleName = parentItem.ModuleName ?? string.Empty,
                             module.Level,
                             module.CreationTime,
                             module.Creator,
                             module.LastModifier,
                             module.LastModifyTime,
                             module.IsDeleted
                         }).OrderBy(item => item.ModuleCode).ToList();
            var moduleList = new List<PermissionModule>();
            list.ForEach(module =>
            {
                moduleList.Add(new PermissionModule
                {
                    Id = module.Id,
                    ModuleCode = module.ModuleCode,
                    ModuleName = module.ModuleName,
                    ParentModuleId = module.ParentModuleId,
                    ParentModuleCode = module.ParentModuleCode,
                    ParentModuleName = module.ParentModuleName,
                    Level = module.Level,
                    CreationTime = module.CreationTime,
                    Creator = module.Creator,
                    LastModifier = module.LastModifier,
                    LastModifyTime = module.LastModifyTime,
                    IsDeleted = module.IsDeleted
                });
            });
            return moduleList;
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
            if (permissionModule.ParentModuleId != null)
            {
                permissionModule.Level = GetPermissionModule(permissionModule.ParentModuleId.Value).Level + 1;
            }
            var updatedItem = GetPermissionModule(permissionModule.Id);
            Mapper.Map(permissionModule, updatedItem);
            return _permissionModuleRepository.Update(updatedItem);
        }
        public void DeleteModule(int id) {
            _permissionModuleRepository.Delete(id);
            var children = new List<PermissionModule>();
            children = GetChildren(id, children);
            foreach(var item in children)
            {
                _permissionModuleRepository.Delete(item.Id);
            }
        }

        private List<PermissionModule> GetChildren(int parentId, List<PermissionModule> moduleList)
        {
            var list = _permissionModuleRepository.GetAll().Where(item => item.ParentModuleId == parentId).ToList();
            foreach(var item in list)
            {
                moduleList.Add(item);
                GetChildren(item.Id, moduleList);
            }
            return moduleList;
        }

    }
}
