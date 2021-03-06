﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;

namespace ZNV.Timesheet.Project
{
    [Table("Project")]
    public class Project : BaseEntity
    {
        [Required(ErrorMessage = "类别不能为空!")]
        public virtual string Category { get; set; }

        [Required(ErrorMessage = "项目编码不能为空!")]
        public virtual string ProjectCode { get; set; }

        [Required(ErrorMessage = "科室名称不能为空!")]
        public virtual string ProjectName { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "项目经理不能为空!")]
        public virtual string ProjectManagerID { get; set; }

        [NotMapped]
        public virtual string ProjectManagerName { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "产品经理不能为空!")]
        public virtual string ProductManagerID { get; set; }

        [NotMapped]
        public virtual string ProductManagerName { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "产品管理不能为空!")]
        public virtual string ProductLeaderID { get; set; }

        [NotMapped]
        public virtual string ProductLeaderName { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "项目类别不能为空！")]
        public virtual string ProjectType { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "项目级别不能为空")]
        public virtual string ProjectLevel { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "项目性质不能为空！")]
        public virtual string ProjectKind { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "产线属性不能为空！")]
        public virtual string ProductionLineAttribute { get; set; }

        [RequiredIf("Category == '立项项目' || Category == '非立项项目'", ErrorMessage = "项目状态不能为空！")]
        public virtual string ProjectStatus { get; set; }

        public virtual bool IsEnabled { get; set; }

        [Required(ErrorMessage = "生效日期不能为空!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? EffectiveDate { get; set; }

        [Required(ErrorMessage = "失效日期不能为空!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ExpirationDate { get; set; }

        [NotMapped]
        public virtual int NextID { get; set; }
    }
}
