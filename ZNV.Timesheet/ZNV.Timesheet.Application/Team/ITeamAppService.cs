using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.Team
{
    public interface ITeamAppService : IApplicationService
    {
        List<Team> GetTeamList();
        List<Team> GetTeamList(int start, int length, string sortColumnName, string sortDirection, string teamName, string teamLeader, string departmentId, out int totalCount);
        Team GetTeam(int id);
        int AddTeam(Team team);
        Team UpdateTeam(Team team);
        void DeleteTeam(int id);
    }
}
