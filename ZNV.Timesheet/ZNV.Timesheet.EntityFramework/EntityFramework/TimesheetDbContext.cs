using System.Data.Common;
using System.Data.Entity;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework
{
    public class TimesheetDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<ZNV.Timesheet.Project.Project> Projects { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public TimesheetDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in TimesheetDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of TimesheetDbContext since ABP automatically handles it.
         */
        public TimesheetDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public TimesheetDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public TimesheetDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}
