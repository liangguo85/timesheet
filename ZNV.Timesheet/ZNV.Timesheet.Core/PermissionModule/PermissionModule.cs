using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.PermissionModule
{
    [Table("PermissionModule")]
    public class PermissionModule : BaseEntity
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
    }
}
