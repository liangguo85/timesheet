using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.RoleManagement
{
    [Table("Role")]
    public class Role : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "角色名称不能为空!")]
        public virtual string RoleName { get; set; }

        [NotMapped]
        public List<HREmployee> Users { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public Role()
        {
            CreationTime = DateTime.Now;
        }
    }
}
