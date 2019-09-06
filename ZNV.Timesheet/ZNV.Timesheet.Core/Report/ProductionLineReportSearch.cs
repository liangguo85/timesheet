using System;
using System.Collections.Generic;

namespace ZNV.Timesheet.Report
{
    public class ProductionLineReportSearch
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public List<string> productionLineList { get; set; }
        public string currentUserID { get; set; }
    }
}
