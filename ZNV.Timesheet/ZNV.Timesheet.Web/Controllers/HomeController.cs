using System.Linq;
using System.Web.Mvc;
using ZNV.Timesheet.Employee;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Team;
using ZNV.Timesheet.Web.Common;

namespace ZNV.Timesheet.Web.Controllers
{
    public class HomeController : TimesheetControllerBase
    {
        private readonly IProjectAppService _projectAppService;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ITeamAppService _teamService;
        public HomeController(IProjectAppService projectAppService, IEmployeeAppService employeeAppService, ITeamAppService teamService)
        {
            _projectAppService = projectAppService;
            _employeeAppService = employeeAppService;
            _teamService = teamService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SSOLogOut()
        {
            HttpContext.Session["OAUserName"] = "";
            CommonHelper.CurrentUser = "";
            return Json(new { success = true, message = "清空Session成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmployeeList(string searchTerm, int pageSize, int pageNum)
        {
            int totalCount = 0;
            var itemList = _employeeAppService.GetEmployeeList(searchTerm, pageSize, pageNum, out totalCount);
            var result = new
            {
                Total = totalCount,
                Results = itemList
            };
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetDepartmentList(string searchTerm, int pageSize, int pageNum)
        {
            int totalCount = 0;
            var itemList = _employeeAppService.GetDepartmentList(searchTerm, pageSize, pageNum, out totalCount);
            var result = new
            {
                Total = totalCount,
                Results = itemList
            };
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetTeamList(string searchTerm, int pageSize, int pageNum)
        {
            var itemList = _teamService.GetTeamList().Where(x => string.IsNullOrEmpty(searchTerm) || x.TeamName.Contains(searchTerm)).ToList();
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

        [HttpGet]
        public ActionResult GetProjectList(string searchTerm, int pageSize, int pageNum)
        {
            var itemList = _projectAppService.GetAllValidProjectList().Where(x => string.IsNullOrEmpty(searchTerm) || x.ProjectName.Contains(searchTerm) || x.ProjectCode.Contains(searchTerm)).ToList();
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

        [HttpGet]
        public ActionResult GetAllProjectList(string searchTerm, int pageSize, int pageNum)
        {
            var itemList = _projectAppService.GetAllProjectList().Where(x => string.IsNullOrEmpty(searchTerm) || x.ProjectName.Contains(searchTerm) || x.ProjectCode.Contains(searchTerm)).ToList();
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