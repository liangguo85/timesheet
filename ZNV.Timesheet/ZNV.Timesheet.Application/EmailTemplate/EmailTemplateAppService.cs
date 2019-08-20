using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;

namespace ZNV.Timesheet.EmailTemplate
{
    public class EmailTemplateAppService : TimesheetAppServiceBase, IEmailTemplateAppService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailTemplateAppService(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }
        public List<EmailTemplate> GetEmailTemplateList()
        {
            return _emailTemplateRepository.GetAllList();
        }
        public List<EmailTemplate> GetEmailTemplateList(int start, int length, string sortColumnName, string sortDirection, out int totalCount)
        {
            totalCount = _emailTemplateRepository.Count();
            return _emailTemplateRepository.GetAll().OrderBy(sortColumnName + " " + sortDirection).Skip(start).Take(length).ToList();
        }
        public EmailTemplate GetEmailTemplate(int id)
        {
            return _emailTemplateRepository.GetAll().Where(item => item.Id == id).FirstOrDefault();
        }
        public int AddEmailTemplate(EmailTemplate emailTemplate)
        {
            return _emailTemplateRepository.InsertAndGetId(emailTemplate);
        }
        public EmailTemplate UpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            var updatedEmailTemplate = GetEmailTemplate(emailTemplate.Id);
            Mapper.Map(emailTemplate, updatedEmailTemplate);
            return _emailTemplateRepository.Update(updatedEmailTemplate);
        }
        public void DeleteEmailTemplate(int id)
        {
            _emailTemplateRepository.Delete(id);
        }
    }
}
