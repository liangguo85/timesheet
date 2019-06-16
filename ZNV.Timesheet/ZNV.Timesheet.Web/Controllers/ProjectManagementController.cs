using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Project;

namespace ZNV.Timesheet.Web.Controllers
{
    public class ProjectManagementController : TimesheetControllerBase
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
            var projects = _projectAppService.GetAllProjectList();
            return Json(new { sEcho=1, iTotalRecords = 2, iTotalDisplayRecords = 2, aaData = projects });
        }
    }
}