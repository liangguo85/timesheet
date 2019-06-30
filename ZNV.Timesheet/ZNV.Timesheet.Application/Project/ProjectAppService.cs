using System.Collections.Generic;
using System.Linq;

namespace ZNV.Timesheet.Project
{
    public class ProjectAppService: TimesheetAppServiceBase, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        //private readonly IRepository<Holiday>

        public ProjectAppService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public void CreateProject(Project project)
        {
            _projectRepository.Insert(project);
        }

        public List<Project> GetAllProjectList()
        {
            return _projectRepository.GetAll().ToList();
        }

        public void UpdateProject(Project project)
        {
            _projectRepository.Update(project);
        }

        public void DeleteProject(int id)
        {
            _projectRepository.Delete(id);
        }
    }
}
