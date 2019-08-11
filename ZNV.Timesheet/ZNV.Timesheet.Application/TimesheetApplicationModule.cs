using System.Collections.Generic;
using System.Reflection;
using Abp.Configuration;
using Abp.Modules;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(TimesheetCoreModule))]
    public class TimesheetApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            AutoMappperExtension.InitializeAutomapper();
            base.PreInitialize();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
