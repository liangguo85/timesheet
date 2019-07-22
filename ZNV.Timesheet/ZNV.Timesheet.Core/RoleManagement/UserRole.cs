using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.RoleManagement
{
    [Table("UserRole")]
    public class UserRole : Entity<int>, IHasCreationTime
    {
        public virtual string UserId { get; set; }

        public virtual int RoleId { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public UserRole()
        {
            CreationTime = DateTime.Now;
        }
    }
}
