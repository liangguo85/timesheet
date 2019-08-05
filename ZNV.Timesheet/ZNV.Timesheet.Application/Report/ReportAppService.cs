using System;
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

        public DataTable GetDepartmentReport(DateTime startDate, DateTime endDate)
        {
            return _reportRepository.GetDepartmentReport(startDate, endDate);
        }
    }
}
