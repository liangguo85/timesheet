using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.ApproveLog;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class ApproveLogController : Controller
    {
        private readonly ITimesheetAppService _tsService;
        private readonly IApproveLogAppService _alService;

        public ApproveLogController(ITimesheetAppService tsService, IApproveLogAppService alService)
        {
            _tsService = tsService;
            _alService = alService;
        }

        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取审批日志，暂时不用
        /// </summary>
        /// <param name="workflowInstanceID"></param>
        /// <returns></returns>
        [HttpGet]
        private ActionResult GetAllApproveLog(string timesheetIdList)
        {
            List<ApproveLogForComment> alfc = new List<ApproveLogForComment>();
            var tsIdList = timesheetIdList.Split(',');
            List<Timesheet.Timesheet> tsList = new List<Timesheet.Timesheet>();
            foreach (var tsId in tsIdList)
            {
                tsList.Add(_tsService.GetTimesheetsByID(int.Parse(tsId.Trim())));
            }
            string workflowInstanceID = string.Empty;
            var list = _alService.GetApproveLogByWorkflowInstanceID(workflowInstanceID);
            return View(list);
        }

    }
}