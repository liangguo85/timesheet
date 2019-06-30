using ZNV.Timesheet.Employee;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class HRDepartmentRepository : MAPSysDbRepositoryBase<HRDepartment, string>, IHRDepartmentRepository
    {
        public HRDepartmentRepository(IDbContextProvider<MAPSysDbContext> dbContextProvider)
           : base(dbContextProvider)
        {

        }
    }
}
