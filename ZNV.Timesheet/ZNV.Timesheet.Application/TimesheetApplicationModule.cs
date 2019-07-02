using System.Reflection;
using Abp.Modules;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(TimesheetCoreModule))]
    public class TimesheetApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.UnitOfWork.Scope = System.Transactions.TransactionScopeOption.Suppress;
            base.PreInitialize();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
