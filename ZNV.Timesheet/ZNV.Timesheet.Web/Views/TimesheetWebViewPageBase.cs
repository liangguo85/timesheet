using Abp.Web.Mvc.Views;

namespace ZNV.Timesheet.Web.Views
{
    public abstract class TimesheetWebViewPageBase : TimesheetWebViewPageBase<dynamic>
    {

    }

    public abstract class TimesheetWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected TimesheetWebViewPageBase()
        {
            LocalizationSourceName = TimesheetConsts.LocalizationSourceName;
        }
    }
}