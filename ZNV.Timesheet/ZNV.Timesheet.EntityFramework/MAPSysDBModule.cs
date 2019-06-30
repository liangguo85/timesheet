using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using ZNV.Timesheet.EntityFramework;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(TimesheetCoreModule))]
    public class MAPSysDBModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "MAPSysDB";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<MAPSysDbContext>(null);
        }
    }
}
