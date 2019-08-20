using ZNV.Timesheet.RoleManagement;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class RoleDepartmentRepository : TimesheetRepositoryBase<RoleDepartment, int>, IRoleDepartmentRepository
    {
        public RoleDepartmentRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
          : base(dbContextProvider)
        {

        }
    }
}
