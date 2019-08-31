using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Abp.Dependency;
using ZNV.Timesheet.RoleManagement;
using ZNV.Timesheet.Web.Common;

namespace ZNV.Timesheet.Web.App_Start
{
    public class TimesheetAuthorizeAttribute: AuthorizeAttribute, ITransientDependency
    {
        public string ModuleCode { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (String.IsNullOrEmpty(CommonHelper.CurrentUser))
                return false;

            var modules = IocManager.Instance.Resolve<IRoleManagementAppService>().GetEmployeeModules(CommonHelper.CurrentUser);

            return modules.Any(item => item.ModuleCode.StartsWith(ModuleCode));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (String.IsNullOrEmpty(CommonHelper.CurrentUser))
            {
                string casServerUrlPrefix = ConfigurationManager.AppSettings["casServerUrlPrefix"];
                filterContext.Result = new EmptyResult();
                filterContext.HttpContext.Response.Redirect(casServerUrlPrefix + "login?service=" + filterContext.HttpContext.Request.Url);
                filterContext.HttpContext.Response.End();
                return;
            }
            else
            {
                RedirectToRouteResult routeData = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                filterContext.Result = routeData;
            }
        }

    }
}