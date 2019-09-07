using System.Data;
using Abp.Application.Services;

namespace ZNV.Timesheet.Report
{
    public interface IReportAppService : IApplicationService
    {
        DataTable GetDepartmentReport(DepartmentReportSearch search);
        DataTable GetProjectReport(ProjectReportSearch search);
        DataTable GetProjectManpowerReport(ProjectReportSearch search);
        DataTable GetProductionLineReport(ProductionLineReportSearch search);
        DataTable GetNotSubmitTimesheetUserList(string dateList);
        DataTable GetDepartmentManagerList();
    }
}
