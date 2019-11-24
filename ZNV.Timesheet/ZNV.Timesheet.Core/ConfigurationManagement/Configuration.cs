using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.ConfigurationManagement
{
    [Table("Configuration")]
    public class Configuration : BaseEntity
    {
        [Required(ErrorMessage = "配置编号不能为空!")]
        public virtual string ConfigValue { get; set; }

        [Required(ErrorMessage = "配置文本不能为空!")]
        public virtual string ConfigText { get; set; }

        public virtual int? ParentId { get; set; }

        [NotMapped]
        public virtual string ParentConfigValue { get; set; }
        [NotMapped]
        public virtual string ParentConfigText { get; set; }

        public virtual int Level { get; set; }
    }
}
