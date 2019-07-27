using Abp.Web.Mvc.Controllers;
using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Timesheet;

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