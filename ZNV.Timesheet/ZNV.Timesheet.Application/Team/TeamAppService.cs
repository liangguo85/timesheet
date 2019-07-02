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
