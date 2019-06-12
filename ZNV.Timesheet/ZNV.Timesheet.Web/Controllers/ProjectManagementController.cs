using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZNV.Timesheet.Web.Controllers
{
    public class ProjectManagementController : TimesheetControllerBase
    {
        // GET: ProjectManagement
        public ActionResult Index()
        {
            return View();
        }
    }
}