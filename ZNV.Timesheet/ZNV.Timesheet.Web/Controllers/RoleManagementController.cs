using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.RoleManagement;
using System.Linq.Dynamic;

namespace ZNV.Timesheet.Web.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly IRoleManagementAppService _roleManagementAppService;
        public RoleManagementController(IRoleManagementAppService roleManagementAppService)
        {
            _roleManagementAppService = roleManagementAppService;
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
                //ViewBag.Employees = new SelectList(_employeeAppService.GetEmployeeList().Take(10), "EmployeeCode", "EmployeeName");
                //ViewBag.Departments = new SelectList(_employeeAppService.GetDepartmentList().Take(10), "DeptCode1", "DeptName1");
                return View(new Role());
            }
            else
            {
                var role = _roleManagementAppService.GetRole(id);
                //ViewBag.Employees = new SelectList(_employeeAppService.GetEmployeeList().Where(x => x.EmployeeCode == team.TeamLeader).ToList(), "EmployeeCode", "EmployeeName");
                //ViewBag.Departments = new SelectList(_employeeAppService.GetDepartmentList().Where(x => x.DeptCode1 == team.DepartmentID).ToList(), "DeptCode1", "DeptName1");
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
    }
}