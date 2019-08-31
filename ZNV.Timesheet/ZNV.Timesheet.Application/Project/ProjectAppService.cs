using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace ZNV.Timesheet.Project
{
    public class ProjectAppService: TimesheetAppServiceBase, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;

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
            var updatedProject = _projectRepository.GetAll().Where(x => x.Id == project.Id).FirstOrDefault();
            Mapper.Map(project, updatedProject);
            _projectRepository.Update(updatedProject);
        }

        public void DeleteProject(int id)
        {
            _projectRepository.Delete(id);
        }

        public int GetProjectCount()
        {
            return _projectRepository.Count();
        }
    }
}
