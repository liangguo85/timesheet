using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.ApproveLog;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetPendingController : Controller
    {
        private readonly ITimesheetAppService _appService;
        private readonly IProjectAppService _projectService;
        private readonly IApproveLogAppService _alService;
        private readonly Employee.IEmployeeAppService _employeeAppService;
        public TimesheetPendingController(ITimesheetAppService appService,
            IProjectAppService projectService,
            IApproveLogAppService alService,
            Employee.IEmployeeAppService employeeAppService)
        {
            _appService = appService;
            _projectService = projectService;
            _alService = alService;
            _employeeAppService = employeeAppService;
        }

        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllPendingTimesheets()
        {
            string user = Common.CommonHelper.CurrentUser;
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string submitUser = string.Empty;
            var list = _appService.GetAllTimesheets().Where(ts => ts.Status == ApproveStatus.Approving && ts.Approver == user).ToList();
            if (!string.IsNullOrEmpty(Request["columns[1][search][value]"]) && Request["columns[1][search][value]"] != "null")
            {
                submitUser = Request["columns[1][search][value]"];
                list = list.Where(ts => ts.TimesheetUser == submitUser).ToList();
            }
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
                    Remarks = GetApproveLog(g.Key.WorkflowInstanceID),
                    IDList = List2String(g.Select(ts => ts.Id.ToString()).Distinct().ToList()),
                    TimesheetList = g.ToList()
                }).ToList();

            int totalRow = listGroup.Count;
            listGroup = listGroup.Skip(start).Take(length).ToList();
            //listGroup.ForEach(item =>
            //{
            //    var tsUser = _employeeAppService.GetEmployeeByCode(item.TimesheetUser);
            //    item.TimesheetUser = tsUser.EmployeeName + "(" + tsUser.EmployeeCode + ")";
            //});
            return Json(new { data = listGroup, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        private string GetApproveLog(string workflowInstanceID)
        {
            string al = "<p align=\"left\">";
            var alList = _alService.GetApproveLogByWorkflowInstanceID(workflowInstanceID);
            //alList.ForEach(item =>
            //{
            //    var tsUser = _employeeAppService.GetEmployeeByCode(item.CurrentOperator);
            //    item.CurrentOperator = tsUser.EmployeeName + "(" + tsUser.EmployeeCode + ")";
            //});
            for (int i = 0; i < alList.Count; i++)
            {
                al += string.Format("{0}|{1}:{2}<br/>", alList[i].OperateTime.ToString("yyyy-MM-dd"), alList[i].CurrentOperator, alList[i].OperateType);
            }
            al += "</p>";
            return al;
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

        [HttpPost]
        public ActionResult AddOrEdit(Timesheet.Timesheet ts)
        {
            if (string.IsNullOrEmpty(ts.TimesheetUser))
            {
                ts.TimesheetUser = Common.CommonHelper.CurrentUser;
            }
            if (ts.Id == 0)
            {
                ts.Creator = Common.CommonHelper.CurrentUser;
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
        public ActionResult CommApprove(String tsIdList, string comment)
        {
            if (!string.IsNullOrEmpty(tsIdList))
            {
                var operateTime = DateTime.Now;
                var idList = tsIdList.Split(',');
                foreach (var id in idList)
                {
                    var ts = _appService.GetTimesheetsByID(int.Parse(id));
                    ts.Status = ApproveStatus.Approved;
                    ts.Approver = Common.CommonHelper.CurrentUser;
                    ts.ApprovedTime = operateTime;
                    AddOrEdit(ts);
                    _alService.AddApproveLog(new ApproveLog.ApproveLog()
                    {
                        WorkflowInstanceID = ts.WorkflowInstanceID,
                        OperateTime = operateTime,
                        Comment = comment,
                        OperateType = "审批通过",
                        CurrentOperator = Common.CommonHelper.CurrentUser,
                        NextOperator = "",
                        Creator = Common.CommonHelper.CurrentUser
                    });
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
        public ActionResult CommReject(String tsIdList, string comment)
        {
            if (!string.IsNullOrEmpty(tsIdList))
            {
                var operateTime = DateTime.Now;
                var idList = tsIdList.Split(',');
                foreach (var id in idList)
                {
                    var ts = _appService.GetTimesheetsByID(int.Parse(id));
                    ts.Status = ApproveStatus.Draft;
                    ts.Approver = ts.Creator;
                    ts.ApprovedTime = operateTime;
                    AddOrEdit(ts);
                    _alService.AddApproveLog(new ApproveLog.ApproveLog()
                    {
                        WorkflowInstanceID = ts.WorkflowInstanceID,
                        OperateTime = operateTime,
                        Comment = comment,
                        OperateType = "驳回",
                        CurrentOperator = Common.CommonHelper.CurrentUser,
                        NextOperator = "",
                        Creator = Common.CommonHelper.CurrentUser
                    });
                }
                return Json(new { success = true, message = "审批驳回工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要审批驳回的工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SelectTransferUser(string id = null)
        {
            var approveLog = _alService.GetApproveLogByWorkflowInstanceID(id).OrderByDescending(al => al.OperateTime).FirstOrDefault();
            var empList = new List<Employee.HREmployee>() {
                new Employee.HREmployee(){
                     EmployeeCode = approveLog.NextOperator,
                     EmployeeName = approveLog.NextOperator
                }
            };
            ViewBag.Employees = new SelectList(empList, "EmployeeCode", "EmployeeName");
            return View(approveLog);
        }

        [HttpPost]
        public ActionResult SelectTransferUser(string workflowInstanceID, string nextOperator)
        {
            var timeNow = DateTime.Now;
            //先把对应的timesheet的approver都改成转办人
            var targetTimeSheetList = _appService.GetTimesheetsByWorkflowInstanceID(workflowInstanceID);
            foreach (var ts in targetTimeSheetList)
            {
                ts.Approver = nextOperator;
                ts.ApprovedTime = timeNow;
                _appService.UpdateTimesheet(ts);
            }
            //然后再记录审批日志
            _alService.AddApproveLog(new ApproveLog.ApproveLog()
            {
                WorkflowInstanceID = workflowInstanceID,
                OperateTime = DateTime.Now,
                Comment = "转办",
                OperateType = "转办",
                CurrentOperator = Common.CommonHelper.CurrentUser,
                NextOperator = nextOperator,
                Creator = Common.CommonHelper.CurrentUser
            });
            return Json(new { success = true, message = "转办成功!" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 转办
        /// </summary>
        /// <param name="tsIdList">需要转办的工时id列表</param>
        /// <param name="transferUser">需要转办的人员</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommTransfer(String tsIdList, string transferUser)
        {
            if (!string.IsNullOrEmpty(tsIdList))
            {
                var operateTime = DateTime.Now;
                var idList = tsIdList.Split(',');
                List<Timesheet.Timesheet> tsList = new List<Timesheet.Timesheet>();
                foreach (var id in idList)
                {
                    tsList.Add(_appService.GetTimesheetsByID(int.Parse(id)));
                }
                var listGroup = tsList.GroupBy(ts => new { ts.WorkflowInstanceID }).ToList();
                foreach (var wiid in listGroup)
                {
                    SelectTransferUser(wiid.Key.WorkflowInstanceID, transferUser);
                }

                return Json(new { success = true, message = "转办成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要转办的数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}