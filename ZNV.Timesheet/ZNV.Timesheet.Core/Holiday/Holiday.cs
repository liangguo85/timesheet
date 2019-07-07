using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZNV.Timesheet.Holiday
{
    [Table("Holiday")]
    public class Holiday : Entity<int>, IHasCreationTime
    {
        [Required(ErrorMessage = "日期不能为空!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? HolidayDate { get; set; }

        [Required(ErrorMessage = "类型不能为空!")]
        public virtual string HolidayType { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public Holiday()
        {
            CreationTime = DateTime.Now;
        }
    }
}
