using System;
using System.Data.Entity.Migrations;
using ZNV.Timesheet.EntityFramework;

namespace ZNV.Timesheet.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TimesheetDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Timesheet";
        }

        protected override void Seed(TimesheetDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
