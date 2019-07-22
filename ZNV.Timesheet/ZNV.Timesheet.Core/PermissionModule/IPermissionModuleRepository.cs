using Abp.Domain.Repositories;

namespace ZNV.Timesheet.PermissionModule
{
    public interface IPermissionModuleRepository : IRepository<PermissionModule, int>
    {
    }
}
