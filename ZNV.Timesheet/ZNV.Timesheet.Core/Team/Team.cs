using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Team
{
    [Table("Team")]
    public class Team : BaseEntity
    {
        [Required(ErrorMessage = "科室名称不能为空!")]
        public virtual string TeamName { get; set; }

        [Required(ErrorMessage = "所属部门不能为空!")]
        public virtual string DepartmentID { get; set; }

        [NotMapped]
        public virtual string DepartmentName { get; set; }

        [Required(ErrorMessage = "科室领导不能为空!")]
        public virtual string TeamLeader { get; set; }

        [NotMapped]
        public virtual string TeamLeaderName { get; set; }
    }
}
