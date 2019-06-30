using System;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZNV.Timesheet.Employee
{
    [Table("HRDeptTree")]
    public class HRDepartment : Entity<string>
    {
        [Column("FullDeptCode")]
        public override string Id { get; set; }
        public virtual string FullDeptName { get; set; }
        public virtual string IsActiveDept { get; set; }
        public virtual string DeptCode1 { get; set; }
        public virtual string DeptName1 { get; set; }
        public virtual int? DeptLay1 { get; set; }
        public virtual string DeptCode2 { get; set; }
        public virtual string DeptName2 { get; set; }
        public virtual int? DeptLay2 { get; set; }
        public virtual string DeptCode3 { get; set; }
        public virtual string DeptName3 { get; set; }
        public virtual int? DeptLay3 { get; set; }
        public virtual string DeptCode4 { get; set; }
        public virtual string DeptName4 { get; set; }
        public virtual int? DeptLay4 { get; set; }
    }
}
