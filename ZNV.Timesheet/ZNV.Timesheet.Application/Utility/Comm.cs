using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Holiday;

namespace ZNV.Timesheet.Utility
{
    public static class Comm
    {
        /// <summary>
        /// 获取某个日期时间所在周对应的工作日列表
        /// </summary>
        /// <param name="timeNow"></param>
        /// <returns></returns>
        public static List<DateTime> GetWorkDateTimes(DateTime timeNow)
        {
            List<DateTime> dateTimeList = new List<DateTime>();
            if (timeNow.DayOfWeek == DayOfWeek.Sunday)
            {
                timeNow = timeNow.AddDays(-6);
            }
            else
            {
                timeNow = timeNow.AddDays(-((int)timeNow.DayOfWeek - 1));
            }
            var holidayList = IocManager.Instance.Resolve<IHolidayAppService>().GetHolidayList().Where(p => p.HolidayDate.Value.Date >= timeNow.Date && p.HolidayDate.Value.Date < timeNow.Date.AddDays(7));
            for (int i = 0; i < 7; i++)
            {
                var dateTime = timeNow.Date.AddDays(i);
                var holiday = holidayList.Where(h => h.HolidayDate.Value.Date == dateTime.Date).FirstOrDefault();
                if (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (holiday != null && holiday.HolidayType == "工作日")
                    {
                        dateTimeList.Add(dateTime);
                    }
                }
                else
                {
                    if (holiday == null || holiday.HolidayType != "节假日")
                    {
                        dateTimeList.Add(dateTime);
                    }
                }
            }
            return dateTimeList;
        }
    }
}
