using Abp.Domain.Repositories;

namespace ZNV.Timesheet.ApproveLog
{
    public interface IApproveLogRepository : IRepository<ApproveLog, int>
    {
    }
}
