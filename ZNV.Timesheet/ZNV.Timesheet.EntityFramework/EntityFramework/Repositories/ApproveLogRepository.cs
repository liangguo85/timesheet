using System;
using System.Linq;
using ZNV.Timesheet.ApproveLog;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class ApproveLogRepository : TimesheetRepositoryBase<ZNV.Timesheet.ApproveLog.ApproveLog, int>, IApproveLogRepository
    {
        public ApproveLogRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        
    }
}
