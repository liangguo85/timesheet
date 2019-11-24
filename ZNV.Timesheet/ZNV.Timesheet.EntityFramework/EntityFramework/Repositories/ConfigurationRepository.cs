using ZNV.Timesheet.ConfigurationManagement;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class ConfigurationRepository : TimesheetRepositoryBase<Configuration, int>, IConfigurationRepository
    {
        public ConfigurationRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
