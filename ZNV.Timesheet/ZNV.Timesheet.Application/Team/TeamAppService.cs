using System.Collections.Generic;
using System.Linq;

namespace ZNV.Timesheet.Team
{
    public class TeamAppService : TimesheetAppServiceBase, ITeamAppService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamAppService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public List<Team> GetTeamList()
        {
            return _teamRepository.GetAllList();
        }
        public int AddTeam(Team team)
        {
            return _teamRepository.InsertAndGetId(team);
        }
        public Team UpdateTeam(Team team)
        {
            return _teamRepository.Update(team);
        }
        public void DeleteTeam(int id)
        {
            _teamRepository.Delete(id);
        }
    }

}
