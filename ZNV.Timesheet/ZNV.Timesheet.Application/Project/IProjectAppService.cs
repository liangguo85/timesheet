using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace ZNV.Timesheet.Project
{
    public interface IProjectAppService: IApplicationService
    {
        void CreateProject(Project project);
        List<Project> GetAllProjectList();
        void UpdateProject(Project project);
    }
}
