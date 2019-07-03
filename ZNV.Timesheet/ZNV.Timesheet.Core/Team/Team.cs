using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Team
{
    [Table("Team")]
    public class Team : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "科室名称不能为空!")]
        public virtual string TeamName { get; set; }

        [Required(ErrorMessage = "所属部门不能为空!")]
        public virtual string DepartmentID { get; set; }

        [NotMapped]
        public virtual string DepartmentName { get; set; }

        [Required(ErrorMessage = "科室领导不能为空!")]
        public virtual string TeamLeader { get; set; }

        [NotMapped]
        public virtual string TeamLeaderName { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public Team()
        {
            CreationTime = DateTime.Now;
        }
    }
}
