using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Holiday
{
    class HolidayAppService : TimesheetAppServiceBase, IHolidayAppService
    {
        private readonly IHolidayRepository _repository;
        //private readonly IRepository<Holiday>

        public HolidayAppService(IHolidayRepository repository)
        {
            _repository = repository;
        }

        public Holiday GetHolidayByDate(DateTime date)
        {
           return _repository.GetHolidayByDate(date);
        }
    }
}
