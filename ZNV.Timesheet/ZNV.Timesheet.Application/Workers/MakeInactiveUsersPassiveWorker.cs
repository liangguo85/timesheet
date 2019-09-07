using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Project;
using ZNV.Timesheet.Holiday;
using ZNV.Timesheet.Timesheet;
using ZNV.Timesheet.Employee;
using ZNV.Timesheet.Smtp;
using ZNV.Timesheet.Utility;
using ZNV.Timesheet.Report;
using ZNV.Timesheet.EmailTemplate;

namespace ZNV.Timesheet.Workers
{
    public class MakeInactiveUsersPassiveWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public MakeInactiveUsersPassiveWorker(AbpTimer timer)
            : base(timer)
        {
            Timer.Period = 5 * 1000 * 60;//10分钟
            //Timer.Period = 1 * 1000 * 60;//10分钟
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            DateTime timeNow = DateTime.Now;
            //每周四早上9点发送邮件（上周未填写周报、本周未填写周报）
            if (timeNow.DayOfWeek == DayOfWeek.Thursday && timeNow.Hour == 9 && timeNow.Minute > 0 && timeNow.Minute <= 10)
            {//因为执行间隔是10分钟一次，所以判断分钟在0-10之间就能保证只执行一次，即9点11分-9点59分不会执行
                SendTimesheetEmail(timeNow);
            }
            //每周一早上9点自动生成领导的工时
            else if (timeNow.DayOfWeek == DayOfWeek.Monday && timeNow.Hour == 9 && timeNow.Minute > 0 && timeNow.Minute <= 10)
            //else if (timeNow.DayOfWeek == DayOfWeek.Saturday && timeNow.Hour == 16 && timeNow.Minute > 35 && timeNow.Minute <= 36)
            {
                AutoCreateManagerTimesheet(timeNow);
            }
            //其他时间什么事情都不做
            else
            {

            }
        }

        private void SendTimesheetEmail(DateTime timeNow)
        {
            //上周未提交的
            List<DateTime> dateTimeList = Comm.GetWorkDateTimes(timeNow.AddDays(-7));
            var emailTemplate = IocManager.Instance.Resolve<IEmailTemplateAppService>().GetEmailTemplateList().Where(et => et.EmailTemplateCode == EmailType.NotSubmitTimesheetPreWeek).FirstOrDefault();
            if (emailTemplate != null)
            {
                SendNotSubmitTimesheet(dateTimeList, emailTemplate);
            }

            emailTemplate = IocManager.Instance.Resolve<IEmailTemplateAppService>().GetEmailTemplateList().Where(et => et.EmailTemplateCode == EmailType.NotSubmitTimesheetThisWeek).FirstOrDefault();
            if (emailTemplate != null)
            {
                //本周未提交的
                dateTimeList = Comm.GetWorkDateTimes(timeNow);
                SendNotSubmitTimesheet(dateTimeList, emailTemplate);
            }
        }

        private void SendNotSubmitTimesheet(List<DateTime> dateTimeList, EmailTemplate.EmailTemplate emailTemplate)
        {
            string dateTimeListString = string.Empty;
            string EmployeeCode = string.Empty, EmployeeName = string.Empty, Email = string.Empty;
            foreach (var date in dateTimeList)
            {
                dateTimeListString += (string.IsNullOrEmpty(dateTimeListString) ? "" : ",") + date.ToString("yyyy-MM-dd");
            }
            var userList = IocManager.Instance.Resolve<IReportAppService>().GetNotSubmitTimesheetUserList(dateTimeListString);
            if (userList != null && userList.Rows.Count > 0)
            {
                for (int i = 0; i < userList.Rows.Count; i++)
                {
                    EmployeeCode = userList.Rows[i]["EmployeeCode"].ToString();
                    EmployeeName = userList.Rows[i]["EmployeeName"].ToString();
                    Email = userList.Rows[i]["Email"].ToString();
                    //因为邮件模板中可能会把用户的名称放到邮件内容里面，所以这里每个用户都单独发送
                    //如果说邮件的标题内容都和用户无关，那就可以改成统一发送
                    EmailSender.SendEmail(emailTemplate.EmailTemplateName.Replace("[EmployeeCode]",EmployeeCode).Replace("[EmployeeName]",EmployeeName),
                        emailTemplate.EmailTemplateBody.Replace("[EmployeeCode]", EmployeeCode).Replace("[EmployeeName]", EmployeeName),
                        Email);
                }
            }
        }

        private void AutoCreateManagerTimesheet(DateTime timeNow)
        {
            //先拿到所有的部门manager
            var userList = IocManager.Instance.Resolve<IReportAppService>().GetDepartmentManagerList();

            //每个部门的manager都自动创建本周的工时，项目对应名称是“部门管理”的项目，还需要结合节假日
            var project = IocManager.Instance.Resolve<IProjectAppService>().GetAllProjectList().Where(p => p.ProjectName == "部门管理").FirstOrDefault();
            if (project != null)
            {
                List<DateTime> dateTimeList = Comm.GetWorkDateTimes(timeNow);
                var tsAppService = IocManager.Instance.Resolve<ITimesheetAppService>();
                for (int k = 0; k < userList.Rows.Count; k++)
                {
                    for (int i = 0; i < dateTimeList.Count; i++)
                    {
                        Timesheet.Timesheet ts = new Timesheet.Timesheet()
                        {
                            ApprovedTime = timeNow,
                            Approver = "",
                            CreationTime = timeNow,
                            Creator = "",
                            IsDeleted = false,
                            LastModifier = "",
                            LastModifyTime = timeNow,
                            ProjectGroup = "",
                            ProjectID = project.Id,
                            ProjectName = project.ProjectName,
                            Remarks = "自动创建",
                            Status = ApproveStatus.Approved,
                            TimesheetDate = dateTimeList[i],
                            TimesheetUser = userList.Rows[k]["UserNum"].ToString(),
                            WorkContent = "部门管理",
                            WorkflowInstanceID = "",
                            Workload = 8
                        };
                        tsAppService.CreateTimesheet(ts);
                    }
                }
            }
        }

    }
}
