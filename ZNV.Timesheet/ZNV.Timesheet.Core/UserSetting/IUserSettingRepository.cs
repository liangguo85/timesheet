using Abp.Domain.Repositories;

namespace ZNV.Timesheet.UserSetting
{
    public interface IUserSettingRepository : IRepository<UserSetting, int>
    {
    }
}
