using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.RoleManagement;
using System.Linq.Dynamic;
using ZNV.Timesheet.Employee;
using ZNV.Timesheet.Web.Models;
using ZNV.Timesheet.PermissionModule;

namespace ZNV.Timesheet.Web.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly IRoleManagementAppService _roleManagementAppService;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IPermissionModuleAppService _permissionModuleAppService;
        public RoleManagementController(IRoleManagementAppService roleManagementAppService, IEmployeeAppService employeeAppService, IPermissionModuleAppService permissionModuleAppService)
        {
            _roleManagementAppService = roleManagementAppService;
            _employeeAppService = employeeAppService;
            _permissionModuleAppService = permissionModuleAppService;
        }
        // GET: RoleManagement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            int totalRow = 0;
            List<Role> roleList = _roleManagementAppService.GetRoleList(start, length, sortColumnName, sortDirection, out totalRow);
            return Json(new { data = roleList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Role { Users = _employeeAppService.GetEmployeeList().Take(10).ToList() });
            }
            else
            {
                var role = _roleManagementAppService.GetRole(id);
                return View(role);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Role role)
        {
            role.Creator = "0001";
            if (role.Id == 0)
            {
                _roleManagementAppService.AddRole(role);
                return Json(new { success = true, message = "新增角色信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _roleManagementAppService.UpdateRole(role);
                return Json(new { success = true, message = "更新角色信息成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _roleManagementAppService.DeleteRole(id);
            return Json(new { success = true, message = "删除角色信息成功!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Permission(int id)
        {
            var roleModules = _roleManagementAppService.GetRoleModules(id);
            RolePermissionModel model = new RolePermissionModel {
                RoleId = id,
                RoleName = _roleManagementAppService.GetRole(id).RoleName,
                PermissionModules = roleModules,
                AllPermissionModules = _permissionModuleAppService.GetPermissionModuleList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Permission(RolePermissionModel model)
        {
            string creator = "0001";
            _roleManagementAppService.AddRoleModules(model.RoleId, model.PermissionModuleIds ?? new List<int> { }, creator);
            return Json(new { success = true, message = "角色授权成功!" }, JsonRequestBehavior.AllowGet);
        }
    }
}