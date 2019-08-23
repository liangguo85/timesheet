using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZNV.Timesheet.Smtp;

namespace ZNV.Timesheet.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string subject = "请审批[流程与IT部]刘克甲提交的流程：刘克甲提交的timesheet";
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
