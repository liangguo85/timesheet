using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZNV.Timesheet.ConfigurationManagement;
using ZNV.Timesheet.Report;

namespace ZNV.Timesheet.Web.Controllers
{
    public class ProductionLineReportController : Controller
    {
        private readonly IReportAppService _reportAppService;
        private readonly IConfigurationAppService _configurationAppService;

        public ProductionLineReportController(IReportAppService reportAppService, IConfigurationAppService configurationAppService)
        {
            _reportAppService = reportAppService;
            _configurationAppService = configurationAppService;
        }

        // GET: ProductionLineReport
        public ActionResult Index()
        {
            ViewBag.ProductionLineAttributes = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("04"), "ConfigText", "ConfigText");
            return View();
        }

        [HttpPost]
        public ActionResult GetReport(ProductionLineReportSearch search)
        {
            search.currentUserID = Common.CommonHelper.CurrentUser;
            DataTable dt = _reportAppService.GetProductionLineReport(search);
            string JSONresult = JsonConvert.SerializeObject(dt);
            return Json(new { data = JSONresult }, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetExcelForReport(ProductionLineReportSearch search)
        {
            search.currentUserID = Common.CommonHelper.CurrentUser;
            DataTable dt = _reportAppService.GetProductionLineReport(search);
            string sheetName = "产线项目统计报表";
            var book = Common.CommonHelper.CreateHSSFromDataTable(sheetName, dt, new List<int>() { 0 }, true);
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", sheetName + ".xls");
        }
    }
}