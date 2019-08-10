using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.Team
{
    public class TeamAppService : TimesheetAppServiceBase, ITeamAppService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IHREmployeeRepository _employeeRepository;
        private readonly IHRDepartmentRepository _departmentRepository;

        public TeamAppService(ITeamRepository teamRepository, IHREmployeeRepository employeeRepository, IHRDepartmentRepository departmentRepository)
        {
            _teamRepository = teamRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public List<Team> GetTeamList()
        {
            return _teamRepository.GetAllList();
        }
        public Team GetTeam(int id) {
            return _teamRepository.GetAllList().Where(x => x.Id == id).FirstOrDefault();
        }
        public int AddTeam(Team team)
        {
            return _teamRepository.InsertAndGetId(team);
        }
        public Team UpdateTeam(Team team)
        {
            var updatedTeam = GetTeam(team.Id);
            updatedTeam.DepartmentID = team.DepartmentID;
            updatedTeam.TeamLeader = team.TeamLeader;
            updatedTeam.TeamName = team.TeamName;
            updatedTeam.LastModifier = team.LastModifier;
            updatedTeam.LastModifyTime = team.LastModifyTime;
            return _teamRepository.Update(updatedTeam);
        }
        public void DeleteTeam(int id)
        {
            _teamRepository.Delete(id);
        }
    }

}
