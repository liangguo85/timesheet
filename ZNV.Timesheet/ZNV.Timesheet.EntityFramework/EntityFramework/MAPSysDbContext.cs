using System.Data.Common;
using System.Data.Entity;
using Abp.EntityFramework;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.EntityFramework
{
    public class MAPSysDbContext : AbpDbContext
    {
        public virtual IDbSet<HREmployee> Employees { get; set; }
        public virtual IDbSet<HRDepartment> Departments { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public MAPSysDbContext()
            : base("MAPSysDB")
        {

        }
    }
}
