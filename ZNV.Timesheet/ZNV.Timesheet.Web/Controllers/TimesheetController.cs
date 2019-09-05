using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.ApproveLog;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Team;
using ZNV.Timesheet.Timesheet;
using ZNV.Timesheet.UserSetting;
using ZNV.Timesheet.Web.Common;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheetAppService _appService;
        private readonly IProjectAppService _projectService;
        private readonly IApproveLogAppService _alService;
        private readonly Holiday.IHolidayAppService _holidayService;
        private readonly IUserSettingAppService _usService;
        private readonly ITeamAppService _teamService;

        public TimesheetController(ITimesheetAppService appService,
            IProjectAppService projectService,
            IApproveLogAppService alService,
            Holiday.IHolidayAppService holidayService,
            IUserSettingAppService usService,
            ITeamAppService teamService)
        {
            _appService = appService;
            _projectService = projectService;
            _alService = alService;
            _holidayService = holidayService;
            _usService = usService;
            _teamService = teamService;
        }

        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取当前工时的下一个操作人
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        private string GetNextOperator(Timesheet.Timesheet ts)
        {
            //根据项目对应的类型Category判断
            var projectInfo = _projectService.GetAllProjectList().Where(p => p.Id == ts.ProjectID).FirstOrDefault();
            if (projectInfo != null)
            {
                if (projectInfo.Category == "售前售后")
                {//Category是售前售后则走科室审批（先找user对应的team，然后再找team的teamleader）
                    var us = _usService.GetUserSettingList().Where(p => p.UserId == CommonHelper.CurrentUser).FirstOrDefault();
                    if (us != null && us.TeamId != 0)
                    {
                        return _teamService.GetTeam(us.TeamId).TeamLeader;
                    }
                }
                else
                {//Category是其他则走项目的经理，（先找项目，然后找项目的projectmanager）
                    return projectInfo.ProjectManagerID;
                }
            }
            return "";
        }

        [HttpPost]
        public JsonResult GetAllTimesheets()
        {
            string user = Common.CommonHelper.CurrentUser;
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            DateTime? startDate = null, endDate = null;
            if (!string.IsNullOrEmpty(Request["columns[0][search][value]"]))
            {
                string[] paramList = Request["columns[0][search][value]"].Split(',');
                if (!string.IsNullOrEmpty(paramList[0]))
                {
                    startDate = DateTime.Parse(paramList[0]);
                }
                if (!string.IsNullOrEmpty(paramList[1]))
                {
                    endDate = DateTime.Parse(paramList[1]);
                }
            }
            var list = _appService.GetAllTimesheetsByUser(user, startDate, endDate);
            int totalRow = list.Count;
            list = list.Skip(start).Take(length).ToList();
            List<Project.Project> projects = _projectService.GetAllProjectList();
            list.ForEach(item =>
            {
                item.ProjectName = Common.CommonHelper.GetProjectNameByProjectID(projects, item.ProjectID.Value);
            });
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            SetProjectListToViewData();
            if (id == 0)
            {
                return View(new Timesheet.Timesheet() { Status = ApproveStatus.Draft });
            }
            else
            {
                return View(_appService.GetTimesheetsByID(id));
            }
        }

        /// <summary>
        /// 将项目信息放置到ViewData，供前端使用
        /// </summary>
        private void SetProjectListToViewData()
        {
            List<Project.Project> projects = _projectService.GetAllProjectList();
            List<SelectListItem> selectLists = new List<SelectListItem>();
            if (projects != null && projects.Count > 0)
            {
                foreach (var project in projects)
                {
                    selectLists.Add(new SelectListItem() { Value = project.Id.ToString(), Text = project.ProjectName });
                }
            }
            ViewData["Projects"] = selectLists;
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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _appService.DeleteTimesheet(id);
            return Json(new { success = true, message = "删除工时成功!" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取第几周的数据
        /// </summary>
        /// <param name="week">起始和结束日期，例如：2019-06-24到2019-06-30</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrEditForWeek(string id = null)
        {
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.Date;
            if (string.IsNullOrEmpty(id))
            {
                var dateNow = DateTime.Now.Date;
                var dayOfWeek = (int)dateNow.DayOfWeek;
                if (dayOfWeek == 0)
                    dayOfWeek = 7;
                startDate = startDate.AddDays(-1 * dayOfWeek + 1);
                endDate = startDate.AddDays(6);
            }
            else
            {
                startDate = DateTime.Parse(id.Split('到')[0]);
                endDate = DateTime.Parse(id.Split('到')[1]);
            }
            //判断endDate是否为调休成上班，如果是则周日也允许填写工时
            var isSundayWork = _holidayService.GetHolidayByDate(endDate);
            if (isSundayWork == null || isSundayWork.HolidayType != "周末转上班")
            {
                endDate = endDate.AddDays(-1);
            }
            Timesheet.TimesheetForWeek tfw = new TimesheetForWeek();
            tfw.startDate = startDate.ToString("yyyy-MM-dd");
            tfw.endDate = endDate.ToString("yyyy-MM-dd");
            var tss = _appService.GetAllTimesheetsByUser(Common.CommonHelper.CurrentUser, startDate, endDate);

            while (startDate <= endDate)
            {
                if (tss.Find(ts => { return ts.TimesheetDate.Value == startDate; }) == null)
                {//把缺少的日期初始化一个空白的记录
                    tss.Add(new Timesheet.Timesheet()
                    {
                        Id = 0,
                        TimesheetDate = startDate,
                        Status = ApproveStatus.Draft,
                    });
                }
                startDate = startDate.AddDays(1);
            }
            //排序
            tss.Sort((a, b) => a.TimesheetDate.Value.CompareTo(b.TimesheetDate.Value));

            SetProjectListToViewData();
            tfw.TimesheetList = tss;
            return View(tfw);
        }

        /// <summary>
        /// 保存草稿
        /// </summary>
        /// <param name="tsfw">周工时数据</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDraftForWeek(Timesheet.TimesheetForWeek tsfw)
        {
            var us = _usService.GetUserSettingList().Where(p => p.UserId == CommonHelper.CurrentUser).FirstOrDefault();
            if (!(us != null && us.TeamId != 0))
            {
                return Json(new { success = false, message = "请先在个人设置中设置科室!" }, JsonRequestBehavior.AllowGet);
            }
            if (tsfw != null && tsfw.TimesheetList != null && tsfw.TimesheetList.Count > 0)
            {
                foreach (var ts in tsfw.TimesheetList)
                {
                    ts.Status = ApproveStatus.Draft;
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = "保存周工时草稿数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要保存的周工时草稿数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tsfw">周工时数据</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitFormForWeek(Timesheet.TimesheetForWeek tsfw, string comment)
        {
            if (tsfw != null && tsfw.TimesheetList != null && tsfw.TimesheetList.Count > 0)
            {
                var operateTime = DateTime.Now;
                var newWorkflowInstanceID = Guid.NewGuid().ToString();
                foreach (var ts in tsfw.TimesheetList)
                {
                    var nextOperator = GetNextOperator(ts);
                    if (string.IsNullOrEmpty(ts.WorkflowInstanceID))
                    {
                        ts.WorkflowInstanceID = newWorkflowInstanceID;
                    }
                    ts.Status = ApproveStatus.Approving;
                    ts.Approver = nextOperator;
                    ts.ApprovedTime = operateTime;
                    AddOrEdit(ts);
                    _alService.AddApproveLog(new ApproveLog.ApproveLog()
                    {
                        WorkflowInstanceID = ts.WorkflowInstanceID,
                        OperateTime = operateTime,
                        Comment = comment,
                        OperateType = "提交",
                        CurrentOperator = Common.CommonHelper.CurrentUser,
                        NextOperator = nextOperator,
                        Creator = Common.CommonHelper.CurrentUser
                    });
                }
                return Json(new { success = true, message = "提交周工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要提交周工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 撤回工时
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommRollBack(Timesheet.TimesheetForWeek tsfw, string comment)
        {
            if (tsfw != null && tsfw.TimesheetList != null && tsfw.TimesheetList.Count > 0)
            {
                var operateTime = DateTime.Now;
                foreach (var ts in tsfw.TimesheetList)
                {
                    ts.Status = ApproveStatus.Draft;
                    ts.Approver = ts.Creator;
                    ts.ApprovedTime = operateTime;
                    AddOrEdit(ts);
                    _alService.AddApproveLog(new ApproveLog.ApproveLog()
                    {
                        WorkflowInstanceID = ts.WorkflowInstanceID,
                        OperateTime = operateTime,
                        Comment = comment,
                        OperateType = "撤回",
                        CurrentOperator = Common.CommonHelper.CurrentUser,
                        NextOperator = "",
                        Creator = Common.CommonHelper.CurrentUser
                    });
                }
                return Json(new { success = true, message = "撤回周工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要撤回周工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetProjectList(string searchTerm, int pageSize, int pageNum)
        {
            var itemList = _projectService.GetAllProjectList().Where(x => string.IsNullOrEmpty(searchTerm) || x.ProjectName.Contains(searchTerm) || x.ProjectCode.Contains(searchTerm)).ToList();
            var result = new
            {
                Total = itemList.Count(),
                Results = itemList.Skip((pageNum - 1) * pageSize).Take(pageSize)
            };
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}