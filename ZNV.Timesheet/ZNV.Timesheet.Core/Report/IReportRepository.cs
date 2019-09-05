using System;
using System.Data;

namespace ZNV.Timesheet.Report
{
    public interface IReportRepository 
    {
        DataTable GetDepartmentReport(DepartmentReportSearch search);
        DataTable GetNotSubmitTimesheetUserList(string dateList);
    }
}
