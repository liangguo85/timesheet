using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.EmailTemplate
{
    public interface IEmailTemplateAppService: IApplicationService
    {
        List<EmailTemplate> GetEmailTemplateList();
        List<EmailTemplate> GetEmailTemplateList(int start, int length, string sortColumnName, string sortDirection, out int totalCount);
        EmailTemplate GetEmailTemplate(int id);
        int AddEmailTemplate(EmailTemplate emailTemplate);
        EmailTemplate UpdateEmailTemplate(EmailTemplate emailTemplate);
        void DeleteEmailTemplate(int id);
    }
}
