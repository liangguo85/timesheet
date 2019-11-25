using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;

namespace ZNV.Timesheet.ConfigurationManagement
{
    public class ConfigurationAppService : TimesheetAppServiceBase, IConfigurationAppService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationAppService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }
        public List<Configuration> GetConfigurationList()
        {
            var list = (
                         from config in _configurationRepository.GetAll()
                         join parentModule in _configurationRepository.GetAll() on config.ParentId equals parentModule.Id into gj
                         from parentItem in gj.DefaultIfEmpty()
                         select new
                         {
                             config.Id,
                             config.ConfigValue,
                             config.ConfigText,
                             config.ParentId,
                             ParentConfigValue = parentItem.ConfigValue ?? string.Empty,
                             ParentConfigText = parentItem.ConfigText ?? string.Empty,
                             config.Level,
                             config.CreationTime,
                             config.Creator,
                             config.LastModifier,
                             config.LastModifyTime,
                             config.IsDeleted,
                             OrderId = (parentItem.ConfigValue ?? string.Empty) + config.ConfigValue
                         }).OrderBy(item => item.OrderId).ToList();
            var configList = new List<Configuration>();
            list.ForEach(module =>
            {
                configList.Add(new Configuration
                {
                    Id = module.Id,
                    ConfigValue = module.ConfigValue,
                    ConfigText = module.ConfigText,
                    ParentId = module.ParentId,
                    ParentConfigValue = module.ParentConfigValue,
                    ParentConfigText = module.ParentConfigText,
                    Level = module.Level,
                    CreationTime = module.CreationTime,
                    Creator = module.Creator,
                    LastModifier = module.LastModifier,
                    LastModifyTime = module.LastModifyTime,
                    IsDeleted = module.IsDeleted
                });
            });
            return configList;
        }
        public List<Configuration> GetConfigurationList(int start, int length, string sortColumnName, string sortDirection, out int totalCount)
        {
            totalCount = _configurationRepository.Count();
            return _configurationRepository.GetAll().OrderBy(sortColumnName + " " + sortDirection).Skip(start).Take(length).ToList();
        }
        public Configuration GetConfiguration(int id)
        {
            return _configurationRepository.GetAll().Where(item => item.Id == id).FirstOrDefault();
        }
        public List<Configuration> GetConfigurationByParentConfigValue(string parentConfigValue)
        {
            return GetConfigurationList().Where(item => item.ParentConfigValue == parentConfigValue).ToList();
        }
        public int AddConfiguration(Configuration configuration)
        {
            if (configuration.ParentId != null)
            {
                configuration.Level = GetConfiguration(configuration.ParentId.Value).Level + 1;
            }
            return _configurationRepository.InsertAndGetId(configuration);
        }
        public Configuration UpdateConfiguration(Configuration configuration)
        {
            if (configuration.ParentId != null)
            {
                configuration.Level = GetConfiguration(configuration.ParentId.Value).Level + 1;
            }
            var updatedItem = GetConfiguration(configuration.Id);
            Mapper.Map(configuration, updatedItem);
            return _configurationRepository.Update(updatedItem);
        }
        public void DeleteConfiguration(int id)
        {
            _configurationRepository.Delete(id);
            var children = new List<Configuration>();
            children = GetChildren(id, children);
            foreach (var item in children)
            {
                _configurationRepository.Delete(item.Id);
            }
        }

        private List<Configuration> GetChildren(int parentId, List<Configuration> configList)
        {
            var list = _configurationRepository.GetAll().Where(item => item.ParentId == parentId).ToList();
            foreach (var item in list)
            {
                configList.Add(item);
                GetChildren(item.Id, configList);
            }
            return configList;
        }
    }
}
