using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace ZNV.Timesheet.ConfigurationManagement
{
    public interface IConfigurationAppService : IApplicationService
    {
        List<Configuration> GetConfigurationList();
        List<Configuration> GetConfigurationList(int start, int length, string sortColumnName, string sortDirection, out int totalCount);
        Configuration GetConfiguration(int id);
        int AddConfiguration(Configuration configuration);
        Configuration UpdateConfiguration(Configuration configuration);
        void DeleteConfiguration(int id);
    }
}
