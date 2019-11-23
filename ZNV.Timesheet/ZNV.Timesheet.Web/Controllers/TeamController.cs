using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZNV.Timesheet.Team;
using ZNV.Timesheet.Employee;
using System.Linq.Dynamic;
using ZNV.Timesheet.Web.App_Start;

namespace ZNV.Timesheet.Web.Controllers
{
    [TimesheetAuthorize(ModuleCode = "00010001")]
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
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            int totalRow = 0;
            List<Team.Team> teamList = _teamAppService.GetTeamList(start, length, sortColumnName, sortDirection, Request["columns[0][search][value]"],
                Request["columns[1][search][value]"], Request["columns[2][search][value]"], out totalRow);
            return Json(new { data = teamList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.Employees = new SelectList(_employeeAppService.GetEmployeeList().Take(10), "EmployeeCode", "EmployeeName");
                ViewBag.Departments = new SelectList(_employeeAppService.GetDepartmentList().Take(10), "DeptCode1", "DeptName1");
                return View(new Team.Team());
            }
            else
            {
                var team = _teamAppService.GetTeamList().Where(x => x.Id == id).FirstOrDefault();
                ViewBag.Employees = new SelectList(_employeeAppService.GetEmployeeList().Where(x=>x.EmployeeCode == team.TeamLeader).ToList(), "EmployeeCode", "EmployeeName");
                ViewBag.Departments = new SelectList(_employeeAppService.GetDepartmentList().Where(x => x.DeptCode1 == team.DepartmentID).ToList(), "DeptCode1", "DeptName1");
                return View(team);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Team.Team team)
        {
            if (team.Id == 0)
            {
                team.Creator = Common.CommonHelper.CurrentUser;
                team.LastModifier = team.Creator;
                team.LastModifyTime = DateTime.Now;
                _teamAppService.AddTeam(team);
                return Json(new { success = true, message = "新增科室信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                team.LastModifier = Common.CommonHelper.CurrentUser;
                team.LastModifyTime = DateTime.Now;
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
    }
}