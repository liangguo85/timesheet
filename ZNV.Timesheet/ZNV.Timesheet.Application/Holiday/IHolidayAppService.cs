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
        List<Holiday> GetHolidayList();
        int AddHoliday(Holiday holiday);
        Holiday UpdateHoliday(Holiday holiday);
        void DeleteHoliday(int id);
    }
}
