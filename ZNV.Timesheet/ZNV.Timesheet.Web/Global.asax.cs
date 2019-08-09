using System;
using System.Web.Mvc;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;

namespace ZNV.Timesheet.Web
{
    public class MvcApplication : AbpWebApplication<TimesheetWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
            );
            //GlobalFilters.Filters.Add(new TimeSheetAuthorizationFilter());
            base.Application_Start(sender, e);
        }
    }
}
