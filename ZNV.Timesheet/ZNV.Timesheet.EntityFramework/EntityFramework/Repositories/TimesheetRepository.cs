using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Timesheet;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class TimesheetRepository : TimesheetRepositoryBase<ZNV.Timesheet.Timesheet.Timesheet, int>, ITimesheetRepository
    {
        public TimesheetRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        
    }
}
