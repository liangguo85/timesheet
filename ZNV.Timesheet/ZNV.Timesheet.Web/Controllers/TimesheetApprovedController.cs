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

        public TimesheetApprovedController(ITimesheetAppService appService, Project.IProjectAppService projectService)
        {
            _appService = appService;
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
            var list = _appService.GetAllTimesheetsByUser(user, null, null).Where(ts=>ts.Status == "Approved").ToList();
            int totalRow = list.Count;
            list = list.Skip(start).Take(length).ToList();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }
        
    }
}