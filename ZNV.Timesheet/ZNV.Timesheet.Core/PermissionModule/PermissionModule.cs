using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.PermissionModule
{
    [Table("PermissionModule")]
    public class PermissionModule : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "模块编号不能为空!")]
        public virtual string ModuleCode { get; set; }

        [Required(ErrorMessage = "模块名称不能为空!")]
        public virtual string ModuleName { get; set; }

        public virtual int? ParentModuleId { get; set; }

        [NotMapped]
        public virtual string ParentModuleName { get; set; }
        [NotMapped]
        public virtual string ParentModuleCode { get; set; }

        public virtual int Level { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public PermissionModule()
        {
            CreationTime = DateTime.Now;
        }
    }
}
