using ZNV.Timesheet.Employee;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class HREmployeeRepository : TimesheetRepositoryBase<HREmployee, string>, IHREmployeeRepository
    {
        public HREmployeeRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
           : base(dbContextProvider)
        {

        }
    }
}
