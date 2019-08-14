using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.Web.Controllers
{
    public class LayoutController : Controller
    {
        private readonly IEmployeeAppService _empService;

        public LayoutController(IEmployeeAppService empService)
        {
            _empService = empService;
        }

        [ChildActionOnly]
        public PartialViewResult EmployeeInfo()
        {
            var model = _empService.GetEmployeeByCode(Common.CommonHelper.CurrentUser);

            return PartialView("_EmployeeInfo", model);
        }
    }
}