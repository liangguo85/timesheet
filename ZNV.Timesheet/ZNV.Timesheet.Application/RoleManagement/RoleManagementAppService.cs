using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet.Employee;
using System.Linq.Dynamic;
using ZNV.Timesheet.PermissionModule;

namespace ZNV.Timesheet.RoleManagement
{
    public class RoleManagementAppService : TimesheetAppServiceBase, IRoleManagementAppService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IHREmployeeRepository _employeeRepository;
        private readonly IRoleModuleRepository _roleModuleRepository;
        private readonly IPermissionModuleRepository _permissionModuleRepository;

        public RoleManagementAppService(IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, 
            IHREmployeeRepository employeeRepository, IRoleModuleRepository roleModuleRepository, IPermissionModuleRepository permissionModuleRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _employeeRepository = employeeRepository;
            _roleModuleRepository = roleModuleRepository;
            _permissionModuleRepository = permissionModuleRepository;
        }
        public List<Role> GetRoleList(int start, int length, string sortColumnName, string sortDirection, out int totalCount)
        {
            totalCount = _roleRepository.Count();
            //var users = (from user in _userRepository.GetAll()
            //             join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
            //             join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
            //             where role.Name == roleName
            //             select user.Id).Distinct().ToList();
            
            var roles = _roleRepository.GetAll().OrderBy(sortColumnName + " " + sortDirection).Skip(start).Take(length).ToList();

            var userRoles = (
                         from role in roles
                         join userRole in _userRoleRepository.GetAll() on role.Id equals userRole.RoleId
                         select new { userRole.RoleId, userRole.UserId }).ToList();

            var users = (
                from userRole in userRoles
                join em in _employeeRepository.GetAll() on userRole.UserId equals em.EmployeeCode
                select new { userRole.RoleId, userRole.UserId, em.EmployeeName }
            ).ToList();


            users.ForEach(userRole =>
            {
                roles.ForEach(role =>
                {
                    if (role.Id == userRole.RoleId)
                    {
                        if (role.Users == null)
                        {
                            role.Users = new List<HREmployee>();
                        }
                        role.Users.Add(new HREmployee { EmployeeCode = userRole.UserId, EmployeeName = userRole.EmployeeName });
                        if(role.UserIds == null)
                        {
                            role.UserIds = new List<string>();
                        }
                        role.UserIds.Add(userRole.UserId);
                    }
                });
            });

            //var roles = (from role in _roleRepository.GetAll()
            //             let query = (from userRole in _userRoleRepository.GetAll()
            //                          where userRole.RoleId == role.Id
            //                          join em in _employeeRepository.GetAll() on userRole.UserId equals em.EmployeeCode
            //                          select new HREmployee { EmployeeCode = em.EmployeeCode, EmployeeName = em.EmployeeName })
            //             select new Role { Id = role.Id, RoleName = role.RoleName, Users = query.ToList() }).ToList();
          
            return roles;
        }
        public Role GetRole(int id)
        {
            var role = _roleRepository.GetAll().Where(item => item.Id == id).FirstOrDefault();
            var userRoles = _userRoleRepository.GetAll().Where(item => item.RoleId == role.Id).ToList();
            var users = (
                from userRole in userRoles
                join em in _employeeRepository.GetAll() on userRole.UserId equals em.EmployeeCode
                select new HREmployee { EmployeeCode = em.EmployeeCode, EmployeeName = em.EmployeeName }
            ).ToList();

            role.Users = users;
            role.UserIds = users.Select(item => item.EmployeeCode).ToList();
            
            return role;
        }
        public int AddRole(Role role)
        {
            var roleId = _roleRepository.InsertAndGetId(role);
            if (role.UserIds != null && role.UserIds.Count > 0)
            {
                role.UserIds.ForEach(userId =>
                {
                    UserRole userRole = new UserRole
                    {
                        UserId = userId,
                        RoleId = roleId,
                        Creator = role.Creator
                    };
                    _userRoleRepository.Insert(userRole);
                });
            }

            return roleId;
        }
        public Role UpdateRole(Role role)
        {
            _roleRepository.Update(role);
            if (role.UserIds != null && role.UserIds.Count > 0)
            {
                _userRoleRepository.Delete(userRole => userRole.RoleId == role.Id);
                role.UserIds.ForEach(userId =>
                {
                    UserRole userRole = new UserRole
                    {
                        UserId = userId,
                        RoleId = role.Id,
                        Creator = role.Creator
                    };
                    _userRoleRepository.Insert(userRole);
                });
            }
            return role;
        }
        public void DeleteRole(int id)
        {
            _roleRepository.Delete(id);
            _userRoleRepository.Delete(userRole => userRole.RoleId == id);
        }

        public List<PermissionModule.PermissionModule> GetRoleModules(int roleId)
        {
            var modules = (from roleModule in _roleModuleRepository.GetAll()
                           join module in _permissionModuleRepository.GetAll() on roleModule.ModuleId equals module.Id
                           where roleModule.RoleId == roleId
                           select module).Distinct().ToList();
            return modules;
        }

        public void AddRoleModules(int roleId, List<int> moduleIds, string creator)
        {
            _roleModuleRepository.Delete(roleModule => roleModule.RoleId == roleId);
            moduleIds.ForEach(moduleId =>
            {
                RoleModule roleModule = new RoleModule
                {
                    RoleId = roleId,
                    ModuleId = moduleId,
                    Creator = creator
                };
                _roleModuleRepository.Insert(roleModule);
            });
        }
    }
}
