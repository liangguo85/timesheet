using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace ZNV.Timesheet.Timesheet
{
    public interface ITimesheetAppService : IApplicationService
    {
        /// <summary>
        /// 通过用户去获取工时列表
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        List<Timesheet> GetAllTimesheetsByUser(string user, DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// 通过用户获取某一天的工时记录，因为同一天可以填多个项目，所以一天有可能返回多个记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        List<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime date);

        /// <summary>
        /// 添加或更新工时记录
        /// </summary>
        /// <param name="timesheetList">需要处理的工时列表</param>
        /// <returns>返回空字符串代表成功，失败则返回失败的原因</returns>
        string InsertOrUpdateTimesheets(List<Timesheet> timesheetList);
    }
}
