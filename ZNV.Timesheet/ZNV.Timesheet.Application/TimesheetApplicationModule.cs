using System.Reflection;
using Abp.Modules;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(TimesheetCoreModule))]
    public class TimesheetApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
