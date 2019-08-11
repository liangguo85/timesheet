using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Project
{
    [Table("Project")]
    public class Project : BaseEntity
    {
        public virtual bool IsApproval { get; set; }

        public virtual string ProjectCode { get; set; }

        public virtual string ProjectName { get; set; }

        public virtual string ProjectManagerID { get; set; }

        [NotMapped]
        public virtual string ProjectManagerName { get; set; }

        public virtual string ProductManagerID { get; set; }

        [NotMapped]
        public virtual string ProductManagerName { get; set; }

        public virtual string ProjectType { get; set; }

        public virtual string ProjectLevel { get; set; }

        public virtual string ProjectKind { get; set; }

        public virtual string ProductionLineAttribute { get; set; }

        public virtual string ProjectStatus { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual DateTime? EffectiveDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }
    }
}
