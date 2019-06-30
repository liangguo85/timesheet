using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Project
{
    public interface IProjectRepository: IRepository<Project, int>
    {
        List<Project> GetAllProjects();
    }
}
