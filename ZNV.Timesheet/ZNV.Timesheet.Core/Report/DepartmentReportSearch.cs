using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Report
{
    public class DepartmentReportSearch
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public List<string> departmentIds { get; set; }
    }
}
