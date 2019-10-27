using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.EmailTemplate;

namespace ZNV.Timesheet.Smtp
{
    public static class EmailSender
    {
        /// <summary>
        /// 公用底层发送邮件方法
        /// </summary>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件正文</param>
        /// <param name="mailTo">接收邮箱</param>
        /// <param name="IsBodyHtml">是否是HTML格式，默认为false</param>
        /// <returns></returns>
        public static bool SendEmail(string Subject, string Body, string mailTo, bool IsBodyHtml = false)
        {
            try
            {
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"]; //SMTP服务器
                string senderDisplayName = ConfigurationManager.AppSettings["senderDisplayName"];
                string mailFrom = ConfigurationManager.AppSettings["mailFrom"]; //登陆用户名，邮箱
                string userPassword = ConfigurationManager.AppSettings["userPassword"];
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
                smtpClient.Host = smtpServer; //指定SMTP服务器
                smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码
                                                                                                  // 发送邮件设置       
                MailMessage mailMessage = new MailMessage(new MailAddress(mailFrom, senderDisplayName), new MailAddress(mailTo)); // 发送人和收件人
                mailMessage.Subject = Subject;//主题
                mailMessage.Body = Body;//内容
                mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
                mailMessage.IsBodyHtml = IsBodyHtml;//设置为HTML格式
                mailMessage.Priority = MailPriority.Low;//优先级

                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SendEmailForSubmitToApprover(string submitterTeamName, string submitterName, string approverName, string mailTo, string comments, DateTime? dateTimeNow,string startDate ="",string endDate="")
        {
            var emailTemplate = IocManager.Instance.Resolve<IEmailTemplateAppService>().GetEmailTemplateList()
                .Where(et => et.EmailTemplateCode == EmailType.SubmitToApprover).FirstOrDefault();
            if (emailTemplate == null)
            {
                return;
            }
            CommSendEmail(submitterTeamName, submitterName, approverName, mailTo, comments, dateTimeNow, emailTemplate,startDate,endDate);
        }

        public static void SendEmailForRollbackToSubmitter(string submitterTeamName, string submitterName, string approverName, string mailTo, string comments, DateTime? dateTimeNow)
        {
            var emailTemplate = IocManager.Instance.Resolve<IEmailTemplateAppService>().GetEmailTemplateList()
                .Where(et => et.EmailTemplateCode == EmailType.RollbackToSubmitter).FirstOrDefault();
            if (emailTemplate == null)
            {
                return;
            }
            CommSendEmail(submitterTeamName, submitterName, approverName, mailTo, comments, dateTimeNow, emailTemplate);
        }

        public static void SendEmailForApproverCompleteApprove(string submitterTeamName, string submitterName, string approverName, string mailTo, string comments, DateTime? dateTimeNow)
        {
            var emailTemplate = IocManager.Instance.Resolve<IEmailTemplateAppService>().GetEmailTemplateList()
                .Where(et => et.EmailTemplateCode == EmailType.ApproverCompleteApprove).FirstOrDefault();
            if (emailTemplate == null)
            {
                return;
            }
            CommSendEmail(submitterTeamName, submitterName, approverName, mailTo, comments, dateTimeNow, emailTemplate);
        }

        public static void SendEmailForApproverTransferToOther(string submitterTeamName, string submitterName, string approverName, string mailTo, string comments, DateTime? dateTimeNow)
        {
            var emailTemplate = IocManager.Instance.Resolve<IEmailTemplateAppService>().GetEmailTemplateList()
                .Where(et => et.EmailTemplateCode == EmailType.ApproverTransferToOther).FirstOrDefault();
            if (emailTemplate == null)
            {
                return;
            }
            CommSendEmail(submitterTeamName, submitterName, approverName, mailTo, comments, dateTimeNow, emailTemplate);
        }

        private static void CommSendEmail(string submitterTeamName, string submitterName, string approverName, string mailTo, string comments, DateTime? dateTimeNow, EmailTemplate.EmailTemplate emailTemplate, string startDate = "", string endDate = "")
        {
            if (!dateTimeNow.HasValue)
            {
                dateTimeNow = DateTime.Now;
            }
            string emailSubjectTemplate = emailTemplate.EmailTemplateName;
            string emailBodyTemplate = emailTemplate.EmailTemplateBody;
            string subject = emailSubjectTemplate.Replace("[TeamName]", submitterTeamName)
                .Replace("[Submitter]", submitterName);
            string body = emailBodyTemplate.Replace("[Approver]", approverName)
                .Replace("[Submitter]", submitterName)
                .Replace("[EmailTime]", dateTimeNow.Value.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("[Comments]", comments)
                .Replace("[StartDate]", startDate)
                .Replace("[EndDate]", endDate);
            SendEmail(subject, body, mailTo, false);
        }
    }

    /// <summary>
    /// 邮件类型，每种邮件类型都固定一个编码，通过编码去邮件模板表中获取邮件模板
    /// </summary>
    public static class EmailType
    {
        /// <summary>
        /// 上周有未提交的工时
        /// </summary>
        public static string NotSubmitTimesheetPreWeek = "0001";

        /// <summary>
        /// 本周有未提交的工时
        /// </summary>
        public static string NotSubmitTimesheetThisWeek = "0002";

        /// <summary>
        /// 申请人提交到审批人
        /// </summary>
        public static string SubmitToApprover = "0003";

        /// <summary>
        /// 审批人驳回给申请人
        /// </summary>
        public static string RollbackToSubmitter = "0004";

        /// <summary>
        /// 审批人完成审批
        /// </summary>
        public static string ApproverCompleteApprove = "0005";

        /// <summary>
        /// 审批人将申请转办给其他人
        /// </summary>
        public static string ApproverTransferToOther = "0006";

    }
}
