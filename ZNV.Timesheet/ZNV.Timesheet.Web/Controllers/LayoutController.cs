using System.Web.Mvc;
using ZNV.Timesheet.Employee;
using ZNV.Timesheet.RoleManagement;

namespace ZNV.Timesheet.Web.Controllers
{
    public class LayoutController : Controller
    {
        private readonly IEmployeeAppService _empService;
        private readonly IRoleManagementAppService _roleManagementAppService;

        public LayoutController(IEmployeeAppService empService, IRoleManagementAppService roleManagementAppService)
        {
            _empService = empService;
            _roleManagementAppService = roleManagementAppService;
        }

        [ChildActionOnly]
        public PartialViewResult EmployeeInfo()
        {
            var model = _empService.GetEmployeeByCode(Common.CommonHelper.CurrentUser);

            return PartialView("_EmployeeInfo", model);
        }

        [ChildActionOnly]
        public PartialViewResult Menu()
        {
            var model = _roleManagementAppService.GetEmployeeModules(Common.CommonHelper.CurrentUser);

            return PartialView("_Menu", model);
        }
    }
}