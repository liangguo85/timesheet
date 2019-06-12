using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using ZNV.Timesheet.EntityFramework;

namespace ZNV.Timesheet
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(TimesheetCoreModule))]
    public class TimesheetDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<TimesheetDbContext>(null);
        }
    }
}
