using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Holiday
{
    [Table("Holiday")]
    public class Holiday : Entity<int>, IHasCreationTime
    {
        public virtual DateTime? HolidayDate { get; set; }

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
