using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZNV.Timesheet.Report;

namespace ZNV.Timesheet.Web.Controllers
{
    public class ProjectReportController : Controller
    {
        private readonly IReportAppService _reportAppService;

        public ProjectReportController(IReportAppService reportAppService)
        {
            _reportAppService = reportAppService;
        }

        // GET: ProjectReport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetProjectReport(ProjectReportSearch search)
        {
            search.currentUserID = Common.CommonHelper.CurrentUser;
            DataTable dt = _reportAppService.GetProjectReport(search);
            string JSONresult = JsonConvert.SerializeObject(dt);
            return Json(new { data = JSONresult }, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetExcelForReport(ProjectReportSearch search)
        {
            search.currentUserID = Common.CommonHelper.CurrentUser;
            DataTable dt = _reportAppService.GetProjectReport(search);
            string sheetName = "项目工时统计报表";
            var book = Common.CommonHelper.CreateHSSFromDataTable(sheetName, dt, new List<int>() { 0 }, true);
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", sheetName + ".xls");
        }
    }
}