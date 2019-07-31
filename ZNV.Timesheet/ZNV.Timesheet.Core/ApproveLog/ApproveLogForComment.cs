using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.ApproveLog
{
    public class ApproveLogForComment
    {
        public virtual string WorkflowInstanceID { get; set; }
        
        public virtual List<ApproveLog> ApproveLogList { get; set; }
    }
}
