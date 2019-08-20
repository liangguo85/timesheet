using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZNV.Timesheet.RoleManagement
{
    [Table("RoleModule")]
    public class RoleModule : BaseEntity
    {
        [Required(ErrorMessage = "角色不能为空!")]
        public virtual int RoleId { get; set; }

        [NotMapped]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "权限模块不能为空!")]
        public virtual int ModuleId { get; set; }

        [NotMapped]
        public string ModuleName { get; set; }
    }
}
