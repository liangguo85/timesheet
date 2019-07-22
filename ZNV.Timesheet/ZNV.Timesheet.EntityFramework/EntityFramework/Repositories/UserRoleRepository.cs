using ZNV.Timesheet.RoleManagement;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class UserRoleRepository : TimesheetRepositoryBase<UserRole, int>, IUserRoleRepository
    {
        public UserRoleRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
