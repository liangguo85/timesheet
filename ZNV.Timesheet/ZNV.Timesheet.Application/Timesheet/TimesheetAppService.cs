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
        public List<Timesheet> GetAllTimesheets()
        {
            return _repository.GetAll().OrderByDescending(ts => ts.TimesheetDate).ToList();
        }

        public List<Timesheet> GetAllTimesheetsByUser(string user, DateTime? startDate, DateTime? endDate)
        {
            var query = _repository.GetAll();
            if (!string.IsNullOrEmpty(user))
            {
                query = query.Where(ts => ts.TimesheetUser == user);
            }
            if (startDate.HasValue)
            {
                query = query.Where(ts => ts.TimesheetDate >= startDate);
            }
            if (endDate.HasValue)
            {
                query = query.Where(ts => ts.TimesheetDate <= endDate);
            }
            return query.OrderByDescending(ts => ts.TimesheetDate).ToList();
        }

        /// <summary>
        /// 通过ID去获取工时数据
        /// </summary>
        /// <param name="id">用户</param>
        public Timesheet GetTimesheetsByID(int id)
        {
            return _repository.GetAll().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime date)
        {
            var query = _repository.GetAll();
            query = query.Where(ts => ts.TimesheetUser == user && ts.TimesheetDate == date);
            return query.OrderByDescending(ts => ts.LastModifyTime).ToList();
        }

        /// <summary>
        /// 通过WorkflowInstanceID获取工时记录
        /// </summary>
        /// <param name="WorkflowInstanceID">流程审批id</param>
        /// <returns></returns>
        public List<Timesheet> GetTimesheetsByWorkflowInstanceID(string workflowInstanceID)
        {
            return _repository.GetAllList().Where(ts => ts.WorkflowInstanceID == workflowInstanceID).ToList();
        }

        public string InsertOrUpdateTimesheets(List<Timesheet> timesheetList)
        {
            StringBuilder sb = new StringBuilder();
            if (timesheetList != null && timesheetList.Count > 0)
            {
                try
                {
                    for (int i = 0; i < timesheetList.Count; i++)
                    {
                        _repository.InsertOrUpdate(timesheetList[i]);
                    }
                }
                catch (Exception ex)
                {
                    sb.Append(ex.Message);
                }
            }
            return sb.ToString();
        }

        public void CreateTimesheet(Timesheet ts)
        {
            _repository.Insert(ts);
        }

        public void UpdateTimesheet(Timesheet ts)
        {
            _repository.Update(ts);
        }

        public void DeleteTimesheet(int id)
        {
            _repository.Delete(id);
        }
    }
}
