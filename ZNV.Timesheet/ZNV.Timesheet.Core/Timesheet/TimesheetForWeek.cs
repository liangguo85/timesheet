using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Timesheet
{
    public class TimesheetForWeek
    {
        public virtual string startDate { get; set; }

        public virtual string endDate { get; set; }

        /// <summary>
        /// 某个星期的所有timesheet
        /// </summary>
        public virtual List<Timesheet> TimesheetList { get; set; }
        
    }
}
