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
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            SetProjectListToViewData();
            if (id == 0)
            {
                return View(new Timesheet.Timesheet());
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
                        TimesheetDate = startDate
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
        /// 添加或更新周工时数据
        /// </summary>
        /// <param name="tsfw">周工时数据</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEditForWeek(Timesheet.TimesheetForWeek tsfw)
        {
            if (tsfw != null && tsfw.TimesheetList != null && tsfw.TimesheetList.Count > 0)
            {
                foreach (var ts in tsfw.TimesheetList)
                {
                    AddOrEdit(ts);
                }
                return Json(new { success = true, message = "添加或更新周工时数据成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "需要添加或更新周工时数据为空!" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}