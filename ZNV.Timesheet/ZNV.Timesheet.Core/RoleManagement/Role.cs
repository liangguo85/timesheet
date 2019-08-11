using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ZNV.Timesheet.Employee;

namespace ZNV.Timesheet.RoleManagement
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        [Required(ErrorMessage = "角色名称不能为空!")]
        public virtual string RoleName { get; set; }

        [NotMapped]
        public List<HREmployee> Users { get; set; }

        [NotMapped]
        public List<string> UserIds { get; set; }
    }
}
