using ZNV.Timesheet.Employee;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class HRDepartmentRepository : TimesheetRepositoryBase<HRDepartment, string>, IHRDepartmentRepository
    {
        public HRDepartmentRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
           : base(dbContextProvider)
        {

        }
    }
}
