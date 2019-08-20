using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZNV.Timesheet.EmailTemplate
{
    [Table("EmailTemplate")]
    public class EmailTemplate : BaseEntity
    {
        [Required(ErrorMessage = "邮件模板编码不能为空!")]
        public virtual string EmailTemplateCode { get; set; }

        [Required(ErrorMessage = "邮件模板名称不能为空!")]
        public virtual string EmailTemplateName { get; set; }

        [Required(ErrorMessage = "邮件模板不能为空!")]
        public virtual string EmailTemplateBody { get; set; }
    }
}
