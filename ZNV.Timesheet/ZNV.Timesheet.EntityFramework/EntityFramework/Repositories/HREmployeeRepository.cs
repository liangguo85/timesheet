using ZNV.Timesheet.Employee;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class HREmployeeRepository : MAPSysDbRepositoryBase<HREmployee, string>, IHREmployeeRepository
    {
        public HREmployeeRepository(IDbContextProvider<MAPSysDbContext> dbContextProvider)
           : base(dbContextProvider)
        {

        }
    }
}
