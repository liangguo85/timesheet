using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet;

namespace ZNV.Timesheet.ApproveLog
{
    public class ApproveLogAppService : TimesheetAppServiceBase, IApproveLogAppService
    {
        private readonly IApproveLogRepository _alRepository;

        public ApproveLogAppService(IApproveLogRepository alRepository)
        {
            _alRepository = alRepository;
        }
        public List<ApproveLog> GetApproveLogList()
        {
            return _alRepository.GetAllList();
        }
        public List<ApproveLog> GetApproveLogByWorkflowInstanceID(string workflowInstanceID)
        {
            return _alRepository.GetAllList().Where(al => al.WorkflowInstanceID == workflowInstanceID).OrderBy(al => al.OperateTime).ToList();
        }
        /// <summary>
        /// 添加时，如果对应的WorkflowInstanceID+OperateTime已经存在则跳过
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public int AddApproveLog(ApproveLog al)
        {
            var isExists = _alRepository.GetAllList().Where(w => w.WorkflowInstanceID == al.WorkflowInstanceID && w.OperateTime == al.OperateTime).ToList();
            if (isExists.Count == 0)
            {
                return _alRepository.InsertAndGetId(al);
            }
            else
            {
                return isExists[0].Id;
            }
        }
        public ApproveLog UpdateApproveLog(ApproveLog al)
        {
            return _alRepository.Update(al);
        }
        public void DeleteApproveLog(int id)
        {
            _alRepository.Delete(id);
        }

    }

}
