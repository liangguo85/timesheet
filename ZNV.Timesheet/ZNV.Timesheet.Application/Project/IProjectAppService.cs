using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.Project
{
    public interface IProjectAppService: IApplicationService
    {
        void CreateProject(Project project);
        List<Project> GetAllProjectList();
        void UpdateProject(Project project);
        void DeleteProject(int id);

        int GetProjectCount();
    }
}
