﻿using System;
using System.Collections.Generic;

namespace ZNV.Timesheet.Report
{
    public class DepartmentReportSearch
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public List<string> departmentIds { get; set; }
        public string currentUserID { get; set; }
    }
}
