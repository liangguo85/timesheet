using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ZNV.Timesheet.RoleManagement
{
    [Table("RoleDepartment")]
    public class RoleDepartment : BaseEntity
    {
        [Required(ErrorMessage = "角色不能为空!")]
        public virtual int RoleId { get; set; }


        [Required(ErrorMessage = "部门不能为空!")]
        public virtual string DepartmentId { get; set; }
    }
}
