using System.Data.Entity.Migrations;

namespace ZNV.Timesheet.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Timesheet.EntityFramework.TimesheetDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Timesheet";
        }

        protected override void Seed(Timesheet.EntityFramework.TimesheetDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
