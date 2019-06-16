using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Project;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class ProjectRepository: TimesheetRepositoryBase<ZNV.Timesheet.Project.Project, int>, IProjectRepository
    {
        public ProjectRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<ZNV.Timesheet.Project.Project> GetAllProjects()
        {
            return GetAll().ToList();
        }
    }
}
