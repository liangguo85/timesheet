using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

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
            var updatedHoliday = _holidayRepository.GetAll().Where(x => x.Id == holiday.Id).FirstOrDefault();
            Mapper.Map(holiday, updatedHoliday);
            return _holidayRepository.Update(updatedHoliday);
        }
        public void DeleteHoliday(int id)
        {
            _holidayRepository.Delete(id);
        }
        public Holiday GetHolidayByDate(DateTime date)
        {
            return _holidayRepository.GetHolidayByDate(date);
        }
    }
}
