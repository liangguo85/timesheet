using Abp.Web.Mvc.Controllers;

namespace ZNV.Timesheet.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class TimesheetControllerBase : AbpController
    {
        protected TimesheetControllerBase()
        {
            LocalizationSourceName = TimesheetConsts.LocalizationSourceName;
        }
    }
}