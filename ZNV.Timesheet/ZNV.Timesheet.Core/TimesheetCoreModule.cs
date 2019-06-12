using System.Reflection;
using Abp.Modules;

namespace ZNV.Timesheet
{
    public class TimesheetCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
