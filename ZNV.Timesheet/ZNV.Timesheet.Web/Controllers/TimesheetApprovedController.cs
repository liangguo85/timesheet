using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.ApproveLog;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetApprovedController : Controller
    {
        private readonly ITimesheetAppService _appService;
        private readonly Project.IProjectAppService _projectService;
        private readonly IApproveLogAppService _alService;
        private readonly Employee.IEmployeeAppService _employeeAppService;

        public TimesheetApprovedController(ITimesheetAppService appService, Project.IProjectAppService projectService,
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
        public JsonResult GetAllApprovedTimesheets()
        {
            string user = Common.CommonHelper.CurrentUser;
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string submitUser = string.Empty;
            var list = _appService.GetAllTimesheets().Where(ts => (ts.Status == ApproveStatus.Approved|| ts.Status == ApproveStatus.Rejected) && ts.Approver == user).ToList();
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
                    ProjectName = Common.CommonHelper.List2String(g.Select(ts => ts.ProjectName).Distinct().ToList()),
                    TimesheetDate = Common.CommonHelper.List2String(g.Select(ts => ts.TimesheetDate.Value.ToString("yyyy-MM-dd")).Distinct().ToList()),
                    WorkContent = Common.CommonHelper.List2String(g.Select(ts => ts.WorkContent).Distinct().ToList()),
                    Workload = g.Sum(ts => ts.Workload),
                    Remarks = GetApproveLog(g.Key.WorkflowInstanceID),
                    IDList = Common.CommonHelper.List2String(g.Select(ts => ts.Id.ToString()).Distinct().ToList()),
                    TimesheetList = g.ToList()
                }).ToList();

            int totalRow = listGroup.Count;
            listGroup = listGroup.Skip(start).Take(length).ToList();
            listGroup.ForEach(item =>
            {
                var tsUser = _employeeAppService.GetEmployeeByCode(item.TimesheetUser);
                item.TimesheetUser = tsUser.EmployeeName + "(" + tsUser.EmployeeCode + ")";
            });
            return Json(new { data = listGroup, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        private string GetApproveLog(string workflowInstanceID)
        {
            string al = "";
            var alList = _alService.GetApproveLogByWorkflowInstanceID(workflowInstanceID);
            alList.ForEach(item =>
            {
                var tsUser = _employeeAppService.GetEmployeeByCode(item.CurrentOperator);
                item.CurrentOperatorName = tsUser.EmployeeName + "(" + tsUser.EmployeeCode + ")";
            });
            return Common.CommonHelper.GetApproveLogTreeHtml(alList);
        }
        
    }
}