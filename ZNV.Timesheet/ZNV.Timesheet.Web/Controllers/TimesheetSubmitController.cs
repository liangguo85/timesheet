using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    /// <summary>
    /// 我的已提交，自己可以做撤回操作
    /// </summary>
    public class TimesheetSubmitController : Controller
    {
        private readonly ITimesheetAppService _appService;
        public TimesheetSubmitController(ITimesheetAppService appService)
        {
            _appService = appService;
        }
        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult GetAllSubmitTimesheets(string user)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            var list = _appService.GetAllTimesheetsByUser(user, null, null).Where(ts=>ts.Status == "Pending").ToList();
            int totalRow = list.Count;
            list = list.Skip(start).Take(length).ToList();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Timesheet.Timesheet ts)
        {
            if (string.IsNullOrEmpty(ts.TimesheetUser))
            {
                ts.TimesheetUser = "kojar.liu";
            }
            if (ts.Id == 0)
            {
                ts.Creator = "kojar.liu";
                _appService.CreateTimesheet(ts);
                return Json(new { success = true, message = "新增工时成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _appService.UpdateTimesheet(ts);
                return Json(new { success = true, message = "更新工时成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="tsIdList">需要撤回的工时id列表</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommRollback(String tsIdList)
        {
            if (!string.IsNullOrEmpty(tsIdList))
            {
                var idList = tsIdList.Split(',');
                foreach (var id in idList)
                {
                    var ts = _appService.GetTimesheetsByID(int.Parse(id));
                    ts.Status = "Draft";
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = "撤回工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要撤回的工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}