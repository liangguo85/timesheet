using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.UserSetting
{
    [Table("UserSetting")]
    public class UserSetting : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "员工号不能为空!")]
        public virtual string UserId { get; set; }

        [NotMapped]
        public virtual string  UserName { get; set; }

        [Required(ErrorMessage = "所属科室不能为空!")]
        public virtual int TeamId { get; set; }

        [NotMapped]
        public virtual string TeamName { get; set; }
        
        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public UserSetting()
        {
            CreationTime = DateTime.Now;
        }
    }
}
