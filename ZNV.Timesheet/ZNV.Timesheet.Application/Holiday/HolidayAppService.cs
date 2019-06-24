using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Holiday
{
    public class HolidayAppService : TimesheetAppServiceBase, IHolidayAppService
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidayAppService(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public List<Holiday> GetHolidayList()
        {
            return _holidayRepository.GetAllList();
        }
        public int AddHoliday(Holiday holiday)
        {
            return _holidayRepository.InsertAndGetId(holiday);
        }
        public Holiday UpdateHoliday(Holiday holiday)
        {
            return _holidayRepository.Update(holiday);
        }
        public void DeleteHoliday(int id)
        {
            _holidayRepository.Delete(id);
        }
    }
}
