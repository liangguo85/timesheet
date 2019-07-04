using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Timesheet;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class TimesheetRepository : TimesheetRepositoryBase<ZNV.Timesheet.Timesheet.Timesheet, int>, ITimesheetRepository
    {
        public TimesheetRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        /// <summary>
        /// 通过用户去获取工时列表
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public List<Timesheet.Timesheet> GetAllTimesheetsByUser(string user, DateTime? startDate, DateTime? endDate)
        {
            var query = GetAll();
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
            return query.OrderBy(ts => ts.TimesheetDate).ToList();
        }

        /// <summary>
        /// 通过用户获取某一天的工时记录，因为同一天可以填多个项目，所以一天有可能返回多个记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public List<Timesheet.Timesheet> GetTimesheetsByUserAndDate(string user, DateTime date)
        {
            var query = GetAll();
            query = query.Where(ts => ts.TimesheetUser == user && ts.TimesheetDate == date);
            return query.OrderBy(ts => ts.LastModifyTime).ToList();
        }

        /// <summary>
        /// 添加或更新工时记录
        /// </summary>
        /// <param name="timesheetList">需要处理的工时列表</param>
        /// <returns>返回空字符串代表成功，失败则返回失败的原因</returns>
        public string InsertOrUpdateTimesheets(List<Timesheet.Timesheet> timesheetList)
        {
            StringBuilder sb = new StringBuilder();
            if (timesheetList != null && timesheetList.Count > 0)
            {
                try
                {
                    for (int i = 0; i < timesheetList.Count; i++)
                    {
                        InsertOrUpdate(timesheetList[i]);
                    }
                }
                catch (Exception ex)
                {
                    sb.Append(ex.Message);
                }
            }
            return sb.ToString();
        }
    }
}
