using System.Collections.Generic;
using Abp.Application.Services;

namespace ZNV.Timesheet.ApproveLog
{
    public interface IApproveLogAppService : IApplicationService
    {
        List<ApproveLog> GetApproveLogList();
        List<ApproveLog> GetApproveLogByWorkflowInstanceID(string workflowInstanceID);
        int AddApproveLog(ApproveLog al);
        ApproveLog UpdateApproveLog(ApproveLog al);
        void DeleteApproveLog(int id);
    }
}
