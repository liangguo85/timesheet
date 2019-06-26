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
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            var list = _appService.GetAllTimesheetsByUser(user, startDate, endDate);
            int totalRow = list.Count;
            list = list.Skip(start).Take(length).ToList();
            return Json(new { data = list, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
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
        /// 获取第几周的数据
        /// </summary>
        /// <param name="week">如果=0则返回当前周</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrEditForWeek(int week = 0)
        {
            if (week == 0)
            {
                return View(new Timesheet.Timesheet());
            }
            else
            {
                return View(_appService.GetTimesheetsByID(week));
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Timesheet.Timesheet ts)
        {
            ts.Creator = "kojar.liu";
            if (ts.Id == 0)
            {
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


    }
}