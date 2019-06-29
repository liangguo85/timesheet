using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZNV.Timesheet.Project;
using System.Linq.Dynamic;

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
            if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectManagerID.ToLower().Contains(Request["columns[2][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[3][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProductManagerID.ToLower().Contains(Request["columns[3][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[4][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectType.ToLower().Contains(Request["columns[4][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[5][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectLevel.ToLower().Contains(Request["columns[5][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[6][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectKind.ToLower().Contains(Request["columns[6][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[7][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProductionLineAttribute.ToLower().Contains(Request["columns[7][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[8][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectStatus.ToLower().Contains(Request["columns[8][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[9][search][value]"]))
            {
                projectList = projectList.Where(x => (x.IsApproval ? "是" : "否") == Request["columns[9][search][value]"]).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[10][search][value]"]))
            {
                projectList = projectList.Where(x => (x.IsEnabled ? "是" : "否") == Request["columns[10][search][value]"]).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[11][search][value]"]))
            {
                projectList = projectList.Where(x => x.EffectiveDate.ToString().Contains(Request["columns[11][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[12][search][value]"]))
            {
                projectList = projectList.Where(x => x.ExpirationDate.ToString().Contains(Request["columns[12][search][value]"].ToLower())).ToList();
            }
            int totalRow = projectList.Count;
            projectList = projectList.Skip(start).Take(length).ToList();
            projectList = projectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
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