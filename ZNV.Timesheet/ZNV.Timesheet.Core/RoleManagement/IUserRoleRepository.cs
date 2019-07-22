using System;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.RoleManagement
{
    public interface IUserRoleRepository : IRepository<UserRole, int>
    {
    }
}
