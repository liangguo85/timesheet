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

        /// <summary>
        /// 用户输入的总工时
        /// </summary>
        public virtual decimal AllWorkloadByInput { get; set; }

        /// <summary>
        /// 工作日的总工时，如果用户输入的总工时和工作日总工时不相同则弹出提示
        /// </summary>
        public virtual decimal AllWorkloadByWorkDay { get; set; }
    }
}
