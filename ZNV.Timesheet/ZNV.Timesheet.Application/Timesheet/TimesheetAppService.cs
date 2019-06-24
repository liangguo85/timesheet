using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Timesheet
{
    class TimesheetAppService : TimesheetAppServiceBase, ITimesheetAppService
    {
        private readonly ITimesheetRepository _repository;
        //private readonly IRepository<Holiday>

        public TimesheetAppService(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public List<Timesheet> GetAllTimesheetsByUser(string user, DateTime? startDate, DateTime? endDate)
        {
            return _repository.GetAllTimesheetsByUser(user, startDate, endDate);
        }

        public List<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime date)
        {
            return _repository.GetTimesheetsByUserAndDate(user, date);
        }

        public string InsertOrUpdateTimesheets(List<Timesheet> timesheetList)
        {
            return _repository.InsertOrUpdateTimesheets(timesheetList);
        }
    }
}
