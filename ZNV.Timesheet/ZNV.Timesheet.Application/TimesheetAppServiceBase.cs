using Abp.Application.Services;

namespace ZNV.Timesheet
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class TimesheetAppServiceBase : ApplicationService
    {
        protected TimesheetAppServiceBase()
        {
            LocalizationSourceName = TimesheetConsts.LocalizationSourceName;
        }
    }
}