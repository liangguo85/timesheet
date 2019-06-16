using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Project
{
    public interface IProjectRepository: IRepository<Project, int>
    {
        List<Project> GetAllProjects();
    }
}
