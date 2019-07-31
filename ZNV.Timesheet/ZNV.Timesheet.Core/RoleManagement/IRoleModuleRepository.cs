using System;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.RoleManagement
{
    public interface IRoleModuleRepository : IRepository<RoleModule, int>
    {
    }
}
