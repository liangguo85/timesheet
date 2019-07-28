using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZNV.Timesheet.RoleManagement
{
    [Table("RoleModule")]
    public class RoleModule : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "角色不能为空!")]
        public virtual int RoleId { get; set; }

        [NotMapped]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "权限模块不能为空!")]
        public virtual int ModuleId { get; set; }

        [NotMapped]
        public string ModuleName { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public RoleModule()
        {
            CreationTime = DateTime.Now;
        }
    }
}
