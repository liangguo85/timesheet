using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Smtp
{
    public class EmailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件正文</param>
        /// <param name="mailTo">接收邮箱</param>
        /// <param name="IsBodyHtml">是否是HTML格式，默认为false</param>
        /// <returns></returns>
        public static bool SendEmail(string Subject, string Body, string mailTo, bool IsBodyHtml = false)
        {
            string smtpServer = "smtp.znv.com"; //SMTP服务器
            string senderDisplayName = "ZNV Email Sender";
            string mailFrom = "znvoa@znv.com"; //登陆用户名，邮箱
            string userPassword = "ItAdmin12";
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

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }
    }
}
