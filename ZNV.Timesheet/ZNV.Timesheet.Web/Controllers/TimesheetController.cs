using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Timesheet;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheetAppService _appService;
        private readonly Project.IProjectAppService _projectService;
        public TimesheetController(ITimesheetAppService appService, Project.IProjectAppService projectService)
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
        public JsonResult GetAllTimesheets(string user, DateTime? startDate, DateTime? endDate)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
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
            Timesheet.TimesheetForWeek tfw = new TimesheetForWeek();
            tfw.startDate = startDate.ToString("yyyy-MM-dd");
            tfw.endDate = endDate.ToString("yyyy-MM-dd");
            var tss = _appService.GetAllTimesheetsByUser("kojar.liu", startDate, endDate);

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
        public ActionResult SubmitFormForWeek(Timesheet.TimesheetForWeek tsfw)
        {
            if (tsfw != null && tsfw.TimesheetList != null && tsfw.TimesheetList.Count > 0)
            {
                var newWorkflowInstanceID = Guid.NewGuid().ToString();
                foreach (var ts in tsfw.TimesheetList)
                {
                    ts.WorkflowInstanceID = newWorkflowInstanceID;
                    ts.Status = ApproveStatus.Approving;
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = "提交周工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要提交周工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 提交或撤回工时
        /// </summary>
        /// <param name="id">需要提交或撤回的工时id列表</param>
        /// <param name="actionType">submit是提交，rollback是撤回</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommAction(string tsIdList, string actionType)
        {
            if (string.IsNullOrEmpty(actionType))
            {
                return Json(new { success = true, message = "操作类型未定义!" }, JsonRequestBehavior.AllowGet);
            }
            actionType = actionType.ToLower().Trim();
            if (!(actionType == "submit" || actionType == "rollback"))
            {
                return Json(new { success = true, message = "操作类型异常!" }, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(tsIdList))
            {
                var idList = tsIdList.Split(',');
                foreach (var id in idList)
                {
                    var ts = _appService.GetTimesheetsByID(int.Parse(id));
                    ts.Status = actionType == "submit" ? ApproveStatus.Approving : ApproveStatus.Draft;
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = (actionType == "submit" ? "提交" : "撤回") + "工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要" + (actionType == "submit" ? "提交" : "撤回") + "的工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}