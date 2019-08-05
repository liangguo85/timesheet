using ZNV.Timesheet.UserSetting;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class UserSettingRepository : TimesheetRepositoryBase<UserSetting.UserSetting, int>, IUserSettingRepository
    {
        public UserSettingRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
