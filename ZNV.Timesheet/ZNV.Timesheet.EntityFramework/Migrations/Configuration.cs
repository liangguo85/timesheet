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
                new Timesheet.Timesheet { WorkContent = "��������1", Workload = 8, TimesheetDate = new DateTime(2019, 06, 01), ProjectGroup = "��Ŀ��1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������2", Workload = 8, TimesheetDate = new DateTime(2019, 06, 02), ProjectGroup = "��Ŀ��2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������3", Workload = 8, TimesheetDate = new DateTime(2019, 06, 03), ProjectGroup = "��Ŀ��3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������4", Workload = 8, TimesheetDate = new DateTime(2019, 06, 04), ProjectGroup = "��Ŀ��1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������5", Workload = 8, TimesheetDate = new DateTime(2019, 06, 05), ProjectGroup = "��Ŀ��2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������6", Workload = 8, TimesheetDate = new DateTime(2019, 06, 06), ProjectGroup = "��Ŀ��3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������7", Workload = 8, TimesheetDate = new DateTime(2019, 06, 07), ProjectGroup = "��Ŀ��1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������8", Workload = 8, TimesheetDate = new DateTime(2019, 06, 08), ProjectGroup = "��Ŀ��2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������9", Workload = 8, TimesheetDate = new DateTime(2019, 06, 09), ProjectGroup = "��Ŀ��3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������10", Workload = 8, TimesheetDate = new DateTime(2019, 06, 10), ProjectGroup = "��Ŀ��1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������11", Workload = 8, TimesheetDate = new DateTime(2019, 06, 11), ProjectGroup = "��Ŀ��2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������12", Workload = 8, TimesheetDate = new DateTime(2019, 06, 12), ProjectGroup = "��Ŀ��3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������13", Workload = 8, TimesheetDate = new DateTime(2019, 06, 13), ProjectGroup = "��Ŀ��1", ProjectID = 1, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������14", Workload = 8, TimesheetDate = new DateTime(2019, 06, 14), ProjectGroup = "��Ŀ��2", ProjectID = 2, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user },
                new Timesheet.Timesheet { WorkContent = "��������15", Workload = 8, TimesheetDate = new DateTime(2019, 06, 15), ProjectGroup = "��Ŀ��3", ProjectID = 3, Status = "s", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now, TimesheetUser = user }
            );
            context.Holidays.AddOrUpdate(
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 21), HolidayType = "������ת�ݼ�", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now },
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 22), HolidayType = "��ĩת�ϰ�", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now },
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 28), HolidayType = "������ת�ݼ�", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now },
                new Holiday.Holiday { HolidayDate = new DateTime(2019, 06, 29), HolidayType = "��ĩת�ϰ�", Creator = user, IsDeleted = false, LastModifier = user, LastModifyTime = now }
            );
            context.Projects.AddOrUpdate(
                new Project.Project { ProjectName="��Ŀ1", ProjectCode="1", ProjectType = "��Ŀ����1",  EffectiveDate= new DateTime(2019, 06, 15), ExpirationDate= new DateTime(2019, 12, 15), LastModifier=user, Creator=user, IsEnabled =true, IsDeleted=false },
                new Project.Project { ProjectName = "��Ŀ2", ProjectCode = "2", ProjectType = "��Ŀ����2", EffectiveDate = new DateTime(2019, 06, 15), ExpirationDate = new DateTime(2019, 12, 15), LastModifier = user, Creator = user, IsEnabled = true, IsDeleted = false },
                new Project.Project { ProjectName = "��Ŀ3", ProjectCode = "3", ProjectType = "��Ŀ����3", EffectiveDate = new DateTime(2019, 06, 15), ExpirationDate = new DateTime(2019, 12, 15), LastModifier = user, Creator = user, IsEnabled = true, IsDeleted = false }
            );
        }
    }
}
