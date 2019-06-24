using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
