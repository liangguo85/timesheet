using ZNV.Timesheet.EmailTemplate;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class EmailTemplateRepository : TimesheetRepositoryBase<EmailTemplate.EmailTemplate, int>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
           : base(dbContextProvider)
        {

        }
    }
}
