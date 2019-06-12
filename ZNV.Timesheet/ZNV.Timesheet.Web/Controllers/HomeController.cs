using System.Web.Mvc;

namespace ZNV.Timesheet.Web.Controllers
{
    public class HomeController : TimesheetControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}