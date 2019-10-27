using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.ApproveLog
{
    [Table("ApproveLog")]
    public class ApproveLog : BaseEntity
    {
        [Required(ErrorMessage = "WorkflowInstanceID不能为空!")]
        public virtual string WorkflowInstanceID { get; set; }

        [Required(ErrorMessage = "当前操作人不能为空!")]
        public virtual string CurrentOperator { get; set; }

        [NotMapped]
        public virtual string CurrentOperatorName { get; set; }
        
        public virtual string NextOperator { get; set; }

        [Required(ErrorMessage = "Comment不能为空!")]
        public virtual string Comment { get; set; }

        /// <summary>
        /// 操作类型，提交、撤回、审批通过、驳回、转办等等
        /// </summary>
        public virtual string OperateType { get; set; }

        [Required(ErrorMessage = "OperateTime不能为空!")]
        public virtual DateTime OperateTime { get; set; }
        [NotMapped]
        public virtual string StartDate { get; set; }
        [NotMapped]
        public virtual string EndDate { get; set; }
    }
}
