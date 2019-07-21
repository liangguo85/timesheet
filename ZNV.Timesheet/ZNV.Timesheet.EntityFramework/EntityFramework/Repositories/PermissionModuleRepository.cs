using ZNV.Timesheet.PermissionModule;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class PermissionModuleRepository : TimesheetRepositoryBase<PermissionModule.PermissionModule, int>, IPermissionModuleRepository
    {
        public PermissionModuleRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
