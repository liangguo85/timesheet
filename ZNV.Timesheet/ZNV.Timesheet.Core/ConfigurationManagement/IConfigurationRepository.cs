using Abp.Domain.Repositories;

namespace ZNV.Timesheet.ConfigurationManagement
{
    public interface IConfigurationRepository : IRepository<Configuration, int>
    {
    }
}
