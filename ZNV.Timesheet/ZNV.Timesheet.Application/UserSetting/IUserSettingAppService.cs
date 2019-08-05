using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.UserSetting
{
    public interface IUserSettingAppService : IApplicationService
    {
        List<UserSetting> GetUserSettingList();
        int AddUserSetting(UserSetting us);
        UserSetting UpdateUserSetting(UserSetting us);
        void DeleteUserSetting(int id);
    }
}
