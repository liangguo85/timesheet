using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.Team
{
    public interface ITeamAppService : IApplicationService
    {
        List<Team> GetTeamList();
        int AddTeam(Team team);
        Team UpdateTeam(Team team);
        void DeleteTeam(int id);
    }
}
