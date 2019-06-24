using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace ZNV.Timesheet.Holiday
{
    public interface IHolidayAppService : IApplicationService
    {
        /// <summary>
        /// 获取某一天的假期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        Holiday GetHolidayByDate(DateTime date);
    }
}
