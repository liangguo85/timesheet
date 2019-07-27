using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetApprovedController : Controller
    {
        private readonly ITimesheetAppService _appService;
        private readonly Project.IProjectAppService _projectService;

        public TimesheetApprovedController(ITimesheetAppService appService, Project.IProjectAppService projectService)
        {
            _appService = appService;
            _projectService = projectService;
        }

        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult GetAllApprovedTimesheets(string user)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            var list = _appService.GetAllTimesheetsByUser(user, null, null).Where(ts=>ts.Status == ApproveStatus.Approved).ToList();
            int totalRow = list.Count;
            list = list.Skip(start).Take(length).ToList();
            List<Project.Project> projects = _projectService.GetAllProjectList();
            list.ForEach(item =>
            {
                item.ProjectName = Common.CommonHelper.GetProjectNameByProjectID(projects, item.ProjectID.Value);
            });
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }
        
    }
}