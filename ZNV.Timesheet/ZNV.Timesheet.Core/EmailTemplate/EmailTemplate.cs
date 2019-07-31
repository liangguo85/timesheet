using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZNV.Timesheet.EmailTemplate
{
    [Table("EmailTemplate")]
    public class EmailTemplate : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "邮件模板编码不能为空!")]
        public virtual string EmailTemplateCode { get; set; }

        [Required(ErrorMessage = "邮件模板名称不能为空!")]
        public virtual string EmailTemplateName { get; set; }

        [Required(ErrorMessage = "邮件模板不能为空!")]
        public virtual string EmailTemplateBody { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public EmailTemplate()
        {
            CreationTime = DateTime.Now;
        }
    }
}
