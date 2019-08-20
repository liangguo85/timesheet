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
    }
}
