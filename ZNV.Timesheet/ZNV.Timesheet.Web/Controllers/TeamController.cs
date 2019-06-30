using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZNV.Timesheet.Team;
using ZNV.Timesheet.Employee;
using System.Linq.Dynamic;

namespace ZNV.Timesheet.Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ITeamAppService _teamAppService;
        public TeamController(IEmployeeAppService employeeAppService, ITeamAppService teamAppService)
        {
            _employeeAppService = employeeAppService;
            _teamAppService = teamAppService;
        }
        // GET: Team
        public ActionResult Index()
        {
            //List<HREmployee> employeeList = _employeeAppService.GetEmployeeList().Where(x => (x.ExitDate != null ? x.ExitDate.Value.AddDays(1) : DateTime.Now.AddDays(1)) > DateTime.Now).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            List<Team.Team> teamList = _teamAppService.GetTeamList();
            //if (!string.IsNullOrEmpty(Request["columns[0][search][value]"]))
            //{
            //    holidayList = holidayList.Where(x => x.HolidayDate.ToString().Contains(Request["columns[0][search][value]"].ToLower())).ToList();
            //}
            //if (!string.IsNullOrEmpty(Request["columns[1][search][value]"]))
            //{
            //    holidayList = holidayList.Where(x => x.HolidayType.ToLower().Contains(Request["columns[1][search][value]"].ToLower())).ToList();
            //}
            int totalRow = teamList.Count;
            teamList = teamList.Skip(start).Take(length).ToList();
            teamList = teamList.OrderBy(sortColumnName + " " + sortDirection).ToList();
            return Json(new { data = teamList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Team.Team());
            }
            else
            {
                return View(_teamAppService.GetTeamList().Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Team.Team team)
        {
            team.Creator = "0001";
            if (team.Id == 0)
            {
                _teamAppService.AddTeam(team);
                return Json(new { success = true, message = "新增科室信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _teamAppService.UpdateTeam(team);
                return Json(new { success = true, message = "更新科室信息成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _teamAppService.DeleteTeam(id);
            return Json(new { success = true, message = "删除科室信息成功!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmployeeList(string searchTerm, int pageSize, int pageNum)
        {
            var itemList = _employeeAppService.GetEmployeeList().Where(x => string.IsNullOrEmpty(searchTerm) || x.EmployeeName.Contains(searchTerm) || x.EmployeeCode.Contains(searchTerm)).ToList();
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