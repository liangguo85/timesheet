using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Team
{
    public interface ITeamRepository : IRepository<Team, int>
    {
    }
}
