using System.Reflection;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(AbpWebApiModule), typeof(TimesheetApplicationModule))]
    public class TimesheetWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(TimesheetApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
