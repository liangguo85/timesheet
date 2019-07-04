using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetPendingController : Controller
    {
        private readonly ITimesheetAppService _appService;
        public TimesheetPendingController(ITimesheetAppService appService, Project.IProjectAppService projectService)
        {
            _appService = appService;
        }
        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllPendingTimesheets(string user)
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
        /// 审批通过
        /// </summary>
        /// <param name="tsIdList">需要审批通过的工时id列表</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommApprove(String tsIdList)
        {
            if (!string.IsNullOrEmpty(tsIdList))
            {
                var idList = tsIdList.Split(',');
                foreach (var id in idList)
                {
                    var ts = _appService.GetTimesheetsByID(int.Parse(id));
                    ts.Status = "Approved";
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = "审批通过工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要审批通过的工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 审批拒绝
        /// </summary>
        /// <param name="tsIdList">需要审批拒绝的工时id列表</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommReject(String tsIdList)
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
                return Json(new { success = true, message = "审批拒绝工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要审批拒绝的工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}