using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.RoleManagement
{
    [Table("UserRole")]
    public class UserRole : BaseEntity
    {
        public virtual string UserId { get; set; }

        public virtual int RoleId { get; set; }
    }
}
