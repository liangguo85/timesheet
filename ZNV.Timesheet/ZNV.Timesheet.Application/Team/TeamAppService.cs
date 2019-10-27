using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
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
            var teamList = (from team in _teamRepository.GetAll()
                            join user in _employeeRepository.GetAll() on team.TeamLeader equals user.EmployeeCode
                            join department in _departmentRepository.GetAll() on team.DepartmentID equals department.DeptCode1
                            select new
                            {
                                Id = team.Id,
                                TeamName = team.TeamName,
                                TeamLeader = team.TeamLeader,
                                TeamLeaderName = user.EmployeeName + "(" + user.EmployeeCode + ")",
                                DepartmentID = team.DepartmentID,
                                DepartmentName = department.FullDeptName + "(" + department.DeptCode1 + ")"
                            }).ToList();

            var list = new List<Team>();
            teamList.ForEach(team =>
            {
                list.Add(new Team
                {
                    Id = team.Id,
                    TeamName = team.TeamName,
                    TeamLeader = team.TeamLeader,
                    TeamLeaderName = team.TeamLeaderName,
                    DepartmentID = team.DepartmentID,
                    DepartmentName = team.DepartmentName
                });
            });

            return list;
        }

        public List<Team> GetTeamList(int start, int length, string sortColumnName, string sortDirection, string teamName, string teamLeader, string departmentId, out int totalCount)
        {
            totalCount = _teamRepository.Count();
            var teamList = (from team in _teamRepository.GetAll()
                         join user in _employeeRepository.GetAll() on team.TeamLeader equals user.EmployeeCode
                         join department in _departmentRepository.GetAll() on team.DepartmentID equals department.DeptCode1
                         where team.TeamName.Contains(string.IsNullOrEmpty(teamName) ? team.TeamName: teamName)
                            & team.TeamLeader.Equals(string.IsNullOrEmpty(teamLeader) ? team.TeamLeader : teamLeader)
                            & team.DepartmentID.Equals(string.IsNullOrEmpty(departmentId) ? team.DepartmentID : departmentId)
                            select new {
                             Id = team.Id,
                             TeamName = team.TeamName,
                             TeamLeader = team.TeamLeader,
                             TeamLeaderName = user.EmployeeName + "(" + user.EmployeeCode + ")",
                             DepartmentID = team.DepartmentID,
                             DepartmentName = department.FullDeptName+ "(" + department.DeptCode1 + ")"
                         }).OrderBy(sortColumnName + " " + sortDirection).Skip(start).Take(length).ToList();

            var list = new List<Team>();
            teamList.ForEach(team =>
            {
                list.Add(new Team
                {
                    Id = team.Id,
                    TeamName = team.TeamName,
                    TeamLeader = team.TeamLeader,
                    TeamLeaderName = team.TeamLeaderName,
                    DepartmentID = team.DepartmentID,
                    DepartmentName = team.DepartmentName
                });
            });

            return list;
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
            Mapper.Map(team, updatedTeam);
            return _teamRepository.Update(updatedTeam);
        }
        public void DeleteTeam(int id)
        {
            _teamRepository.Delete(id);
        }
    }

}
