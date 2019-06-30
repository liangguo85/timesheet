using Abp.Domain.Repositories;

namespace ZNV.Timesheet.Employee
{
    public interface IHREmployeeRepository : IRepository<HREmployee, string>
    {
    }
}
