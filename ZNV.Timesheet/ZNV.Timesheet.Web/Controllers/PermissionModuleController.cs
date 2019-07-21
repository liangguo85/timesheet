using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.PermissionModule;

namespace ZNV.Timesheet.Web.Controllers
{
    public class PermissionModuleController : Controller
    {
        private readonly IPermissionModuleAppService _permissionModuleAppService;
        public PermissionModuleController(IPermissionModuleAppService permissionModuleAppService)
        {
            _permissionModuleAppService = permissionModuleAppService;
        }
        // GET: PermissionModule
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
            List<PermissionModule.PermissionModule> roleList = _permissionModuleAppService.GetPermissionModuleList(start, length, sortColumnName, sortDirection, out totalRow);
            return Json(new { data = roleList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            var list = _permissionModuleAppService.GetPermissionModuleList();
            //list.ForEach(item => { item.ModuleName = GetSpace(item.Level) + item.ModuleName; });
            ViewBag.Modules = new SelectList(list, "ModuleCode", "ModuleName");
            if (id == 0)
            {
                return View(new PermissionModule.PermissionModule());
            }
            else
            {
                var role = _permissionModuleAppService.GetPermissionModule(id);
                return View(role);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(PermissionModule.PermissionModule permissionModule)
        {
            permissionModule.Creator = "0001";
            if (permissionModule.Id == 0)
            {
                _permissionModuleAppService.AddPermissionModule(permissionModule);
                return Json(new { success = true, message = "新增权限模块信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _permissionModuleAppService.UpdatePermissionModule(permissionModule);
                return Json(new { success = true, message = "更新权限模块信息成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _permissionModuleAppService.DeleteRole(id);
            return Json(new { success = true, message = "删除权限模块信息成功!" }, JsonRequestBehavior.AllowGet);
        }

        //private string GetSpace(int level)
        //{
        //    string space = "";
        //    for (int i = 0; i < level; i++)
        //    {
        //        space = space + "    ";
        //    }
        //    return space;
        //}
    }
}