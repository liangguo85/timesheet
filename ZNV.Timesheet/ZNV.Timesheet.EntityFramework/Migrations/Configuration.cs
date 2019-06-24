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
            var user = "kojar.liu";
            var now = DateTime.Now;
            context.Timesheets.AddOrUpdate(
                new Timesheet.Timesheet { WorkContent = "工作内容1", Workload = 8, TimesheetDate = new DateTime(2019, 06, 01), ProjectGroup = "项目组1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容2", Workload = 8, TimesheetDate = new DateTime(2019, 06, 02), ProjectGroup = "项目组2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容3", Workload = 8, TimesheetDate = new DateTime(2019, 06, 03), ProjectGroup = "项目组3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容4", Workload = 8, TimesheetDate = new DateTime(2019, 06, 04), ProjectGroup = "项目组1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容5", Workload = 8, TimesheetDate = new DateTime(2019, 06, 05), ProjectGroup = "项目组2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容6", Workload = 8, TimesheetDate = new DateTime(2019, 06, 06), ProjectGroup = "项目组3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容7", Workload = 8, TimesheetDate = new DateTime(2019, 06, 07), ProjectGroup = "项目组1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容8", Workload = 8, TimesheetDate = new DateTime(2019, 06, 08), ProjectGroup = "项目组2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容9", Workload = 8, TimesheetDate = new DateTime(2019, 06, 09), ProjectGroup = "项目组3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容10", Workload = 8, TimesheetDate = new DateTime(2019, 06, 10), ProjectGroup = "项目组1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容11", Workload = 8, TimesheetDate = new DateTime(2019, 06, 11), ProjectGroup = "项目组2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容12", Workload = 8, TimesheetDate = new DateTime(2019, 06, 12), ProjectGroup = "项目组3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容13", Workload = 8, TimesheetDate = new DateTime(2019, 06, 13), ProjectGroup = "项目组1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容14", Workload = 8, TimesheetDate = new DateTime(2019, 06, 14), ProjectGroup = "项目组2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "工作内容15", Workload = 8, TimesheetDate = new DateTime(2019, 06, 15), ProjectGroup = "项目组3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user }
            );
            context.Holidays.AddOrUpdate(
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 21), HolidayType = "工作日转休假", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now },
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 22), HolidayType = "周末转上班", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now },
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 28), HolidayType = "工作日转休假", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now },
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 29), HolidayType = "周末转上班", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now }
            );
            context.Projects.AddOrUpdate(
                new Project.Project { ProjectName="项目1", ProjectCode="1", ProjectType = "项目类型1",  EffectiveDate= new DateTime(2019, 06, 15), ExpirationDate= new DateTime(2019, 12, 15), LastModifier=user, Creator=user, IsEnabled =true, IsDeleted=false },
                new Project.Project { ProjectName = "项目2", ProjectCode = "2", ProjectType = "项目类型2", EffectiveDate = new DateTime(2019, 06, 15), ExpirationDate = new DateTime(2019, 12, 15), LastModifier = user, Creator = user, IsEnabled = true, IsDeleted = false },
                new Project.Project { ProjectName = "项目3", ProjectCode = "3", ProjectType = "项目类型3", EffectiveDate = new DateTime(2019, 06, 15), ExpirationDate = new DateTime(2019, 12, 15), LastModifier = user, Creator = user, IsEnabled = true, IsDeleted = false }
            );
        }
    }
}
