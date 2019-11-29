using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Report
{
    public class TimesheetReportSearch
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string departmentIds { get; set; }
        public string productionLineList { get; set; }
        public string projectIds { get; set; }
        public string userIds { get; set; }
        public string currentUserID { get; set; }
        public string status { get; set; }
        public bool isPage { get; set; }
        public int pageSize { get; set; }
        public int pageStart { get; set; }
    }
}
