using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNV.Timesheet.Holiday;
using Abp.EntityFramework;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class HolidayRepository : TimesheetRepositoryBase<ZNV.Timesheet.Holiday.Holiday, int>, IHolidayRepository
    {
        public HolidayRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
