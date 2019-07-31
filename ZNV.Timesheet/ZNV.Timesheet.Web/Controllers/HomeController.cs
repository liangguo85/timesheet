using System.Web.Mvc;
using ZNV.Timesheet.Web.Common;

namespace ZNV.Timesheet.Web.Controllers
{
    public class HomeController : TimesheetControllerBase
    {
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
	}
}