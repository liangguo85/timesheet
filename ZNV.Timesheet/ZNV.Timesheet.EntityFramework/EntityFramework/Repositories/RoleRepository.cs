using ZNV.Timesheet.RoleManagement;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class RoleRepository : TimesheetRepositoryBase<Role, int>, IRoleRepository
    {
        public RoleRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
