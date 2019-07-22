using ZNV.Timesheet.Team;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class TeamRepository : TimesheetRepositoryBase<Team.Team, int>, ITeamRepository
    {
        public TeamRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
