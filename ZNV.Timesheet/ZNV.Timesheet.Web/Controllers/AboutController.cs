using System.Web.Mvc;

namespace ZNV.Timesheet.Web.Controllers
{
    public class AboutController : TimesheetControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}