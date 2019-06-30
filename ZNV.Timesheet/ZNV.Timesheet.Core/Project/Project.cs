using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Project
{
    [Table("Project")]
    public class Project : Entity<int>, IHasCreationTime
    {
        public virtual bool IsApproval { get; set; }

        public virtual string ProjectCode { get; set; }

        public virtual string ProjectName { get; set; }

        public virtual string ProjectManagerID { get; set; }

        public virtual string ProductManagerID { get; set; }

        public virtual string ProjectType { get; set; }

        public virtual string ProjectLevel { get; set; }

        public virtual string ProjectKind { get; set; }

        public virtual string ProductionLineAttribute { get; set; }

        public virtual string ProjectStatus { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual DateTime? EffectiveDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual string LastModifier { get; set; }

        public virtual DateTime? LastModifyTime { get; set; }

        public virtual bool IsDeleted { get; set; }

        public Project()
        {
            CreationTime = DateTime.Now;
        }
    }
}
