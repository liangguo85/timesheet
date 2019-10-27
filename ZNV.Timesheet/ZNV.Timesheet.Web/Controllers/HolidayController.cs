using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZNV.Timesheet.Holiday;
using System.Linq.Dynamic;
using ZNV.Timesheet.Web.App_Start;

namespace ZNV.Timesheet.Web.Controllers
{
    [TimesheetAuthorize(ModuleCode = "00010003")]
    public class HolidayController : Controller
    {
        private readonly IHolidayAppService _holidayAppService;
        public HolidayController(IHolidayAppService holidayAppService)
        {
            _holidayAppService = holidayAppService;
        }
        // GET: Holiday
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            List<Holiday.Holiday> holidayList = _holidayAppService.GetHolidayList();
            if (!string.IsNullOrEmpty(Request["columns[0][search][value]"]))
            {
                holidayList = holidayList.Where(x => x.HolidayDate.Value.ToString("yyyy-MM-dd").Equals(Request["columns[0][search][value]"].ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Request["columns[1][search][value]"]))
            {
                holidayList = holidayList.Where(x => x.HolidayType.Equals(Request["columns[1][search][value]"].ToLower())).ToList();
            }
            int totalRow = holidayList.Count;
            holidayList = holidayList.OrderBy(sortColumnName + " " + sortDirection).ToList();
            holidayList = holidayList.Skip(start).Take(length).ToList();
            return Json(new { data = holidayList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Holiday.Holiday());
            }
            else
            {
                return View(_holidayAppService.GetHolidayList().Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Holiday.Holiday holiday)
        {
            if (holiday.Id == 0)
            {
                holiday.Creator = Common.CommonHelper.CurrentUser;
                holiday.LastModifier = holiday.Creator;
                holiday.LastModifyTime = DateTime.Now;
                _holidayAppService.AddHoliday(holiday);
                return Json(new { success = true, message = "新增节假日信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                holiday.LastModifier = Common.CommonHelper.CurrentUser;
                holiday.LastModifyTime = DateTime.Now;
                _holidayAppService.UpdateHoliday(holiday);
                return Json(new { success = true, message = "更新节假日信息成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _holidayAppService.DeleteHoliday(id);
            return Json(new { success = true, message = "删除节假日信息成功!" }, JsonRequestBehavior.AllowGet);
        }
    }
}