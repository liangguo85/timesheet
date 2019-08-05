using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.UserSetting
{
    public class UserSettingAppService : TimesheetAppServiceBase, IUserSettingAppService
    {
        private readonly IUserSettingRepository _usRepository;
        private readonly IHREmployeeRepository _employeeRepository;

        public UserSettingAppService(IUserSettingRepository usRepository, IHREmployeeRepository employeeRepository)
        {
            _usRepository = usRepository;
            _employeeRepository = employeeRepository;
        }
        public List<UserSetting> GetUserSettingList()
        {
            return _usRepository.GetAllList();
        }
        public int AddUserSetting(UserSetting us)
        {
            return _usRepository.InsertAndGetId(us);
        }
        public UserSetting UpdateUserSetting(UserSetting us)
        {
            return _usRepository.Update(us);
        }
        public void DeleteUserSetting(int id)
        {
            _usRepository.Delete(id);
        }
    }

}
