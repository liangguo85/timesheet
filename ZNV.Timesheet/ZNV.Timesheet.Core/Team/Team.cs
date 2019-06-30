using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Team
{
    [Table("Team")]
    public class Team : Entity<int>, IHasCreationTime
    {
        public virtual string TeamName { get; set; }

        public virtual string DepartmentID { get; set; }

        public virtual string TeamLeader { get; set; }

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
