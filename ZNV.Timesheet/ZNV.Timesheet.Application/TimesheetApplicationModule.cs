using System.Collections.Generic;
using System.Reflection;
using Abp.Configuration;
using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using ZNV.Timesheet.Workers;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(TimesheetCoreModule))]
    public class TimesheetApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            AutoMappperExtension.InitializeAutomapper();
            //Configuration.Authorization.Providers.Add<TimesheetAuthorizationProvider>();
            base.PreInitialize();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        public override void PostInitialize()
        {
            //注册后台工作者用于发送未填写周报的邮件
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<MakeInactiveUsersPassiveWorker>());
        }
    }
}
