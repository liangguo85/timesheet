using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Report;

namespace ZNV.Timesheet.Web.Controllers
{
    public class DepartmentReportController : Controller
    {
        private readonly IReportAppService _reportAppService;
        public DepartmentReportController(IReportAppService reportAppService)
        {
            _reportAppService = reportAppService;
        }
        // GET: DepartmentReport
        public ActionResult Index()
        {
            DataTable dt = _reportAppService.GetDepartmentReport(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1));
            return View(dt);
        }
    }
}