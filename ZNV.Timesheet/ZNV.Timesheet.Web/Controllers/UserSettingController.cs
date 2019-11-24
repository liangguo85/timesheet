using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.UserSetting;
using ZNV.Timesheet.Team;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.Web.Controllers
{
    public class UserSettingController : Controller
    {
        private readonly IUserSettingAppService _usService;
        private readonly ITeamAppService _teamService;
        private readonly IEmployeeAppService _empService;

        public UserSettingController(IUserSettingAppService usService, ITeamAppService teamService, IEmployeeAppService empService)
        {
            _usService = usService;
            _teamService = teamService;
            _empService = empService;
        }

        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUserSetting()
        {
            UserSetting.UserSetting us = _usService.GetUserSettingList().Where(p => p.UserId == Common.CommonHelper.CurrentUser).FirstOrDefault();
            int teamId = 0;
            if (us != null)
            {
                teamId = us.TeamId;
                Team.Team team = _teamService.GetTeamList().Where(t => t.Id == teamId).FirstOrDefault();
                if (team != null)
                {
                    us.TeamName = team.TeamName;
                    return Json(new { success = true, TeamName = team.TeamName, TeamId = team.Id }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = true, TeamName = "", TeamId = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit()
        {
            UserSetting.UserSetting us = _usService.GetUserSettingList().Where(p => p.UserId == Common.CommonHelper.CurrentUser).FirstOrDefault();
            int teamId = 0;
            if (us != null)
            {
                teamId = us.TeamId;
                Team.Team team = _teamService.GetTeamList().Where(t => t.Id == teamId).FirstOrDefault();
                if (team != null)
                {
                    us.TeamName = team.TeamName;
                    var teamList = new List<Team.Team>() {
                       team
                    };
                    ViewBag.Teams = new SelectList(teamList, "Id", "TeamName");
                    return View(us);
                }
            }
            var teamListEmpty = new List<Team.Team>() { null };
            ViewBag.Teams = new SelectList(teamListEmpty, "Id", "TeamName");
            return View(us);
        }

        [HttpPost]
        public ActionResult AddOrEdit(int teamId)
        {
            UserSetting.UserSetting us = _usService.GetUserSettingList().Where(p => p.UserId == Common.CommonHelper.CurrentUser).FirstOrDefault();
            if (us == null)
            {
                us = new UserSetting.UserSetting()
                {
                    UserId = Common.CommonHelper.CurrentUser,
                    TeamId = teamId,
                    Creator = Common.CommonHelper.CurrentUser,
                    IsDeleted = false
                };
                _usService.AddUserSetting(us);
            }
            else
            {
                us.TeamId = teamId;
                _usService.UpdateUserSetting(us);
            }
            return Json(new { success = true, message = "设置成功!" }, JsonRequestBehavior.AllowGet);
        }
    }
}