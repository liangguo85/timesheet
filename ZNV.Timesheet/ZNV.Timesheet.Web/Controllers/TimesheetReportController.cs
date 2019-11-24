using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZNV.Timesheet.ConfigurationManagement;
using ZNV.Timesheet.Report;


namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetReportController : Controller
    {
        private readonly IReportAppService _reportAppService;
        private readonly IConfigurationAppService _configurationAppService;

        public TimesheetReportController(IReportAppService reportAppService, IConfigurationAppService configurationAppService)
        {
            _reportAppService = reportAppService;
            _configurationAppService = configurationAppService;
        }
        // GET: TimesheetReport
        public ActionResult Index()
        {
            ViewBag.ProductionLineAttributes = new SelectList(_configurationAppService.GetConfigurationByParentConfigValue("04"), "ConfigText", "ConfigText");
            return View();
        }

        [HttpPost]
        public JsonResult GetTimesheetReport()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            int totalRow = 0;
            TimesheetReportSearch search = new TimesheetReportSearch();

            search.isPage = true;
            search.pageStart = Convert.ToInt32(Request["start"]);
            search.pageSize = Convert.ToInt32(Request["length"]);

            search.startDate = string.IsNullOrEmpty(Request["columns[0][search][value]"]) ? DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")) : DateTime.Parse(Request["columns[0][search][value]"]);
            search.endDate = string.IsNullOrEmpty(Request["columns[1][search][value]"]) ? DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) : DateTime.Parse(Request["columns[1][search][value]"]);
            if (!string.IsNullOrEmpty(Request["columns[2][search][value]"]))
            {
                search.departmentIds = Request["columns[2][search][value]"];
            }
            if (!string.IsNullOrEmpty(Request["columns[3][search][value]"]))
            {
                search.productionLineList = Request["columns[3][search][value]"];
            }
            if (!string.IsNullOrEmpty(Request["columns[4][search][value]"]))
            {
                search.projectIds = Request["columns[4][search][value]"];
            }
            if (!string.IsNullOrEmpty(Request["columns[5][search][value]"]))
            {
                search.userIds = Request["columns[5][search][value]"];
            }
            search.currentUserID = Common.CommonHelper.CurrentUser;

            DataTable dt = _reportAppService.GetTimesheetReport(search, out totalRow);

            return Json(new { data = GetListByDataTable(dt), draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetExcelForReport(TimesheetReportSearch search)
        {
            int totalRow = 0;
            search.currentUserID = Common.CommonHelper.CurrentUser;
            search.isPage = false;
            DataTable dt = _reportAppService.GetTimesheetReport(search, out totalRow);
            string sheetName = "员工工时报表";
            var book = Common.CommonHelper.CreateHSSFromDataTable(sheetName, dt, new List<int>() { 0 });
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", sheetName + ".xls");
        }

        private List<object> GetListByDataTable(DataTable dt)
        {
            var result = (from rw in dt.AsEnumerable()
                          select new
                          {
                              DeptName = Convert.ToString(rw["部门"]),
                              EmployeeName = Convert.ToString(rw["员工"]),
                              ProjectCode = Convert.ToString(rw["项目编码"]),
                              ProjectName = Convert.ToString(rw["项目名称"]),
                              ProductionLineAttribute = Convert.ToString(rw["产品线"]),
                              ProductManager = Convert.ToString(rw["产品经理"]),
                              ProjectManager = Convert.ToString(rw["项目经理"]),
                              TimesheetDate = Convert.ToDateTime(rw["日期"]),
                              Workload = Convert.ToString(rw["工时"]),
                              Status = Convert.ToString(rw["状态"]),
                              WorkContent = Convert.ToString(rw["工作内容"]),
                              Remarks = Convert.ToString(rw["备注"]),
                          }).ToList();
            return result.ConvertAll<object>(o => (object)o);
        }
    }
}