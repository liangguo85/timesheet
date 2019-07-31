using ZNV.Timesheet.RoleManagement;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class RoleModuleRepository : TimesheetRepositoryBase<RoleModule, int>, IRoleModuleRepository
    {
        public RoleModuleRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
           : base(dbContextProvider)
        {

        }
    }
}
