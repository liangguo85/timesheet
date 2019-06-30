using System;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Holiday
{
    public interface IHolidayRepository : IRepository<Holiday, int>
    {
        /// <summary>
        /// 获取某一天的假期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        Holiday GetHolidayByDate(DateTime date);
    }
}
