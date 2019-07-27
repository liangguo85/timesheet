using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ZNV.Timesheet.Project;

namespace ZNV.Timesheet.Timesheet
{
    [Table("Timesheet")]
    public class Timesheet : Entity<int>, IHasCreationTime
    {
        public virtual string TimesheetUser { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime? TimesheetDate { get; set; }

        public virtual int? ProjectID { get; set; }

        [NotMapped]
        public virtual string ProjectName { get; set; }

        public virtual string ProjectGroup { get; set; }

        public virtual decimal? Workload { get; set; }

        public virtual string WorkContent { get; set; }

        public virtual string Remarks { get; set; }

        public virtual ApproveStatus Status { get; set; }

        public virtual string Approver { get; set; }

        public virtual DateTime? ApprovedTime { get; set; }

        public virtual string WorkflowInstanceID { get; set; }
        
        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public Timesheet()
        {
            CreationTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ApproveStatus {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft=0,
        /// <summary>
        /// 审核中
        /// </summary>
        Approving = 1,
        /// <summary>
        /// 审批通过
        /// </summary>
        Approved = 2
    }
}

