using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Holiday
{
    public interface IHolidayRepository : IRepository<Holiday, int>
    {
    }
}
