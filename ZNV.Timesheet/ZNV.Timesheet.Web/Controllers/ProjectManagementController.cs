using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Employee;
using System.Linq.Dynamic;
using ZNV.Timesheet.Web.App_Start;
using ZNV.Timesheet.Utility;
using ZNV.Timesheet.ConfigurationManagement;

namespace ZNV.Timesheet.Web.Controllers
{
    [TimesheetAuthorize(ModuleCode = "00010002")]
    public class ProjectManagementController : Controller
    {
        private readonly IProjectAppService _projectAppService;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IConfigurationAppService _configurationAppService;
        public ProjectManagementController(IProjectAppService projectAppService, IEmployeeAppService employeeAppService, IConfigurationAppService configurationAppService)
        {
            _projectAppService = projectAppService;
            _employeeAppService = employeeAppService;
            _configurationAppService = configurationAppService;
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
                projectList = projectList.Where(x => x.ProjectCode == Request["columns[0][search][value]"]).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[1][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProjectManagerID == Request["columns[1][search][value]"]).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProductManagerID == Request["columns[2][search][value]"]).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[3][search][value]"]))
            {
                projectList = projectList.Where(x => x.ProductLeaderID == Request["columns[3][search][value]"]).ToList();
            }
            int totalRow = projectList.Count;
            projectList = projectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
            projectList = projectList.Skip(start).Take(length).ToList();
            projectList.ForEach(item => {
                var projectManager = _employeeAppService.GetEmployeeByCode(item.ProjectManagerID);
                var productManager = _employeeAppService.GetEmployeeByCode(item.ProductManagerID);
                item.ProjectManagerName = projectManager.EmployeeName + "(" + projectManager.EmployeeCode + ")";
                item.ProductManagerName = productManager.EmployeeName + "(" + productManager.EmployeeCode + ")";
            });
            return Json(new { data = projectList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            ViewBag.ProjectTypes = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("01"), "ConfigText", "ConfigText");
            ViewBag.ProjectKinds = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("02"), "ConfigText", "ConfigText");
            ViewBag.ProjectLevels = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("03"), "ConfigText", "ConfigText");
            ViewBag.ProductionLineAttributes = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("04"), "ConfigText", "ConfigText");
            ViewBag.ProjectStatusList = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("05"), "ConfigText", "ConfigText");
            if (id == 0)
            {
                ViewBag.Employees = new SelectList(_employeeAppService.GetEmployeeList().Take(10), "EmployeeCode", "EmployeeName");
                return View(new Project.Project { NextID = UniqueIDGenerator.GetNextID() });
            }
            else
            {
                var project = _projectAppService.GetAllProjectList().Where(x => x.Id == id).FirstOrDefault();
                ViewBag.Employees = new SelectList(_employeeAppService.GetEmployeeList().Where(x => x.EmployeeCode == project.ProductManagerID 
                    || x.EmployeeCode == project.ProjectManagerID 
                    || x.EmployeeCode == project.ProductLeaderID
                ).ToList(), "EmployeeCode", "EmployeeName");
                return View(project);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Project.Project project)
        {
            if (project.Id == 0)
            {
                project.Creator = Common.CommonHelper.CurrentUser;
                project.LastModifier = project.Creator;
                project.LastModifyTime = DateTime.Now;
                _projectAppService.CreateProject(project);
                return Json(new { success = true, message = "新增项目成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                project.LastModifier = Common.CommonHelper.CurrentUser;
                project.LastModifyTime = DateTime.Now;
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