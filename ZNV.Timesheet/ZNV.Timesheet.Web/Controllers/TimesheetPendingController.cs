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
        private readonly Project.IProjectAppService _projectService;
        public TimesheetPendingController(ITimesheetAppService appService, Project.IProjectAppService projectService)
        {
            _appService = appService;
            _projectService = projectService;
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
            var list = _appService.GetAllTimesheetsByUser(user, null, null).Where(ts => ts.Status == ApproveStatus.Approving).ToList();
            List<Project.Project> projects = _projectService.GetAllProjectList();
            foreach (var ts in list)
            {
                ts.ProjectName = Common.CommonHelper.GetProjectNameByProjectID(projects, ts.ProjectID.Value);
            }
            var listGroup = list.GroupBy(ts => new { ts.TimesheetUser, ts.WorkflowInstanceID })
                .Select(g => new TimesheetForPending()
                {
                    TimesheetUser = g.Key.TimesheetUser,
                    WorkflowInstanceID = g.Key.WorkflowInstanceID,
                    ProjectName = List2String(g.Select(ts => ts.ProjectName).Distinct().ToList()),
                    TimesheetDate = List2String(g.Select(ts => ts.TimesheetDate.Value.ToString("yyyy-MM-dd")).Distinct().ToList()),
                    WorkContent = List2String(g.Select(ts => ts.WorkContent).Distinct().ToList()),
                    Workload = g.Sum(ts => ts.Workload),
                    Remarks = List2String(g.Select(ts => ts.Remarks).Distinct().ToList()),
                    IDList = List2String(g.Select(ts => ts.Id.ToString()).Distinct().ToList()),
                    TimesheetList = g.ToList()
                }).ToList();

            int totalRow = listGroup.Count;
            listGroup = listGroup.Skip(start).Take(length).ToList();
            return Json(new { data = listGroup, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        private string List2String(List<string> charList)
        {
            string list = string.Empty;
            if (charList != null && charList.Count > 0)
            {
                for (int i = 0; i < charList.Count; i++)
                {
                    list += string.Format("{0}{1}", (list == "" ? "" : ","), charList[i]);
                }
            }
            return list;
        }
        
        public string GetRemarkFromTimesheetList(List<Timesheet.Timesheet> tsList)
        {
            string remark = string.Empty;
            if (tsList != null && tsList.Count > 0)
            {
                for (int i = 0; i < tsList.Count; i++)
                {
                    remark += string.Format("{0}{1}", (remark == "" ? "" : ","), tsList[i].TimesheetDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            return remark;
        }

        public string GetIDListFromTimesheetList(List<Timesheet.Timesheet> tsList)
        {
            string IDList = string.Empty;
            if (tsList != null && tsList.Count > 0)
            {
                for (int i = 0; i < tsList.Count; i++)
                {
                    IDList += string.Format("{0}{1}", (IDList == "" ? "" : ","), tsList[i].Id);
                }
            }
            return IDList;
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
                    ts.Status = ApproveStatus.Approved;
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
        /// 审批驳回
        /// </summary>
        /// <param name="tsIdList">需要审批驳回的工时id列表</param>
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
                    ts.Status = ApproveStatus.Draft;
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = "审批驳回工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要审批驳回的工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}