using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheetAppService _appService;
        public TimesheetController(ITimesheetAppService appService)
        {
            _appService = appService;
        }
        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllTimesheets(string user, DateTime? startDate, DateTime? endDate)
        {
            var list = _appService.GetAllTimesheetsByUser(user, startDate, endDate);
            return Json(new { sEcho = 1, iTotalRecords = 2, iTotalDisplayRecords = 2, aaData = list });
        }
    }
}