using System;
using System.Data;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Report
{
    public interface IReportRepository 
    {
        DataTable GetDepartmentReport(DateTime startDate, DateTime endDate);
    }
}
