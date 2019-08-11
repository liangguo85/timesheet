using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZNV.Timesheet.Holiday
{
    [Table("Holiday")]
    public class Holiday : BaseEntity
    {
        [Required(ErrorMessage = "日期不能为空!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? HolidayDate { get; set; }

        [Required(ErrorMessage = "类型不能为空!")]
        public virtual string HolidayType { get; set; }
    }
}
