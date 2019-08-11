using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ZNV.Timesheet
{
    public class BaseEntity: Entity<int>, IHasCreationTime
    {
        [NoAutoMappper]
        public virtual string Creator { get; set; }
        [NoAutoMappper]
        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public BaseEntity()
        {
            CreationTime = DateTime.Now;
        }
    }
}
