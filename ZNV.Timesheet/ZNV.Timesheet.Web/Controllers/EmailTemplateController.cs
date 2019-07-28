using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZNV.Timesheet.EmailTemplate;

namespace ZNV.Timesheet.Web.Controllers
{
    public class EmailTemplateController : Controller
    {
        private readonly IEmailTemplateAppService _emailTemplateAppService;
        public EmailTemplateController(IEmailTemplateAppService emailTemplateAppService)
        {
            _emailTemplateAppService = emailTemplateAppService;
        }
        // GET: EmailTemplate
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            int totalRow = 0;
            List<EmailTemplate.EmailTemplate> roleList = _emailTemplateAppService.GetEmailTemplateList(start, length, sortColumnName, sortDirection, out totalRow);
            return Json(new { data = roleList, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRow }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new EmailTemplate.EmailTemplate());
            }
            else
            {
                var role = _emailTemplateAppService.GetEmailTemplate(id);
                return View(role);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(EmailTemplate.EmailTemplate emailTemplate)
        {
            emailTemplate.Creator = "0001";
            if (emailTemplate.Id == 0)
            {
                _emailTemplateAppService.AddEmailTemplate(emailTemplate);
                return Json(new { success = true, message = "新增邮件模板信息成功!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _emailTemplateAppService.UpdateEmailTemplate(emailTemplate);
                return Json(new { success = true, message = "更新邮件模板信息成功!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _emailTemplateAppService.DeleteEmailTemplate(id);
            return Json(new { success = true, message = "删除邮件模板信息成功!" }, JsonRequestBehavior.AllowGet);
        }
    }
}