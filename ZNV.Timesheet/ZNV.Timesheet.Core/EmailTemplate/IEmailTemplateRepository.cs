using System;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.EmailTemplate
{
    public interface IEmailTemplateRepository : IRepository<EmailTemplate, int>
    {
    }
}
