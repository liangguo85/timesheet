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
using ZNV.Timesheet.Smtp;

namespace ZNV.Timesheet.Workers
{
    public class MakeInactiveUsersPassiveWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public MakeInactiveUsersPassiveWorker(AbpTimer timer)
            : base(timer)
        {
            Timer.Period = 5 * 1000 * 60;//10分钟
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            DateTime timeNow = DateTime.Now;
            //每周四早上9点发送邮件（上周未填写周报、本周未填写周报）
            if (!(timeNow.DayOfWeek == DayOfWeek.Thursday && timeNow.Hour == 9 && timeNow.Minute > 0 && timeNow.Minute <= 10))
            {//因为执行间隔是10分钟一次，所以判断分钟在0-10之间就能保证只执行一次，即9点11分-9点59分不会执行
                return;
            }
            string subject = "请审批[流程与IT部]刘克甲提交的流程：刘克甲提交的timesheet:" + timeNow.ToString("yyyy-MM-dd HH:mm:ss");
            string body = @"梁国你好：
      您收到【刘克甲提交的timesheet】的待办，可点击如下链接打开页面审批详细内容：
      访问地址：https://timesheet.znv.com/fs/fsExpenseMain.do?method=view&fdId=16c8387a32d3c6091e435b54887a8325
      流程名称：刘克甲提交的费用报销
      发起时间：2019-08-12";
            string mailTo = "kojargame@sina.com";
            bool isBodyHtml = false;
            EmailSender.SendEmail(subject, body, mailTo, isBodyHtml);
        }
    }
}
