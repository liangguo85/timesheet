using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Web.Models;
using Newtonsoft.Json;

namespace ZNV.Timesheet.Web.Controllers
{
    public class ProjectManagementController : Controller
    {
        private readonly IProjectAppService _projectAppService;
        public ProjectManagementController(IProjectAppService projectAppService)
        {
            _projectAppService = projectAppService;
        }
        // GET: ProjectManagement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllProjects()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<Project.Project> projectList = _projectAppService.GetAllProjectList();

            if (!string.IsNullOrEmpty(Request["columns[0][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectCode.ToLower().Contains(Request["columns[0][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[1][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectName.ToLower().Contains(Request["columns[1][search][value]"].ToLower())).ToList();
            }

            int totalRow = projectList.Count;

            projectList = projectList.Skip(start).Take(length).ToList();
            return Json(new { data = projectList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Project.Project());
            }
            else
            {
                return View(_projectAppService.GetAllProjectList().Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Project.Project project)
        {
            project.Creator = "0001";
            if (project.Id == 0)
            {
                _projectAppService.CreateProject(project);
                return Json(new { success = true, message = "新增项目成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _projectAppService.UpdateProject(project);
                return Json(new { success = true, message = "更新项目成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _projectAppService.DeleteProject(id);
            return Json(new { success = true, message = "删除项目成功!" }, JsonRequestBehavior.AllowGet);
        }
    }
}