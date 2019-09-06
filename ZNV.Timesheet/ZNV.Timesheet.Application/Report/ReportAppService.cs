using System.Data;

namespace ZNV.Timesheet.Report
{
    public class ReportAppService : TimesheetAppServiceBase, IReportAppService
    {
        private readonly IReportRepository _reportRepository;

        public ReportAppService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public DataTable GetDepartmentReport(DepartmentReportSearch search)
        {
            return _reportRepository.GetDepartmentReport(search);
        }
        public DataTable GetProjectReport(ProjectReportSearch search)
        {
            return _reportRepository.GetProjectReport(search);
        }
        public DataTable GetProjectManpowerReport(ProjectReportSearch search)
        {
            return _reportRepository.GetProjectManpowerReport(search);
        }
        public DataTable GetProductionLineReport(ProductionLineReportSearch search)
        {
            return _reportRepository.GetProductionLineReport(search);
        }
        public DataTable GetNotSubmitTimesheetUserList(string dateList)
        {
            return _reportRepository.GetNotSubmitTimesheetUserList(dateList);
        }
    }
}
