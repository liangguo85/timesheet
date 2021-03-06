﻿using System;
using System.Collections.Generic;

namespace ZNV.Timesheet.Report
{
    public class ProjectReportSearch
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public List<string> projectIds { get; set; }
        public string currentUserID { get; set; }
    }
}
