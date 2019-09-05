using System.Data;
using Abp.Application.Services;

namespace ZNV.Timesheet.Report
{
    public interface IReportAppService : IApplicationService
    {
        DataTable GetDepartmentReport(DepartmentReportSearch search);
        DataTable GetNotSubmitTimesheetUserList(string dateList);
    }
}
