using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Holiday;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class HolidayRepository : TimesheetRepositoryBase<ZNV.Timesheet.Holiday.Holiday, int>, IHolidayRepository
    {
        public HolidayRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        /// <summary>
        /// 获取某一天的假期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public ZNV.Timesheet.Holiday.Holiday GetHolidayByDate(DateTime date)
        {
            var query = GetAll();
            query = query.Where(holiday => holiday.HolidayDate == date);
            var list = query.ToList();
            if (list!=null && list.Count>0)
            {
                return list[0];
            }
            //找不到则返回空
            return null;
        }
    }
}
