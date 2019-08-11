using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.UserSetting
{
    [Table("UserSetting")]
    public class UserSetting : BaseEntity
    {
        [Required(ErrorMessage = "员工号不能为空!")]
        public virtual string UserId { get; set; }

        [NotMapped]
        public virtual string  UserName { get; set; }

        [Required(ErrorMessage = "所属科室不能为空!")]
        public virtual int TeamId { get; set; }

        [NotMapped]
        public virtual string TeamName { get; set; }
    }
}
