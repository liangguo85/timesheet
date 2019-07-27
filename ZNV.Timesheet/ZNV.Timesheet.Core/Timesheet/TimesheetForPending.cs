using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Timesheet
{
    public class TimesheetForPending
    {
        /// <summary>
        /// 日期列表，多个以英文逗号隔开
        /// </summary>
        public virtual string TimesheetDate { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public virtual string TimesheetUser { get; set; }

        /// <summary>
        /// id列表，多个以英文逗号隔开
        /// </summary>
        public virtual string IDList { get; set; }

        /// <summary>
        /// id列表，多个以英文逗号隔开
        /// </summary>
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 总工时
        /// </summary>
        public virtual decimal? Workload { get; set; }

        /// <summary>
        /// 工作内容列表，相同的只出现一次
        /// </summary>
        public virtual string WorkContent { get; set; }

        /// <summary>
        /// 备注列表，相同的只出现一次
        /// </summary>
        public virtual string Remarks { get; set; }

        /// <summary>
        /// 流程id
        /// </summary>
        public virtual string WorkflowInstanceID { get; set; }

        /// <summary>
        /// 某个用户以及对应流程的所有timesheet
        /// </summary>
        public virtual List<Timesheet> TimesheetList { get; set; }
    }
}
