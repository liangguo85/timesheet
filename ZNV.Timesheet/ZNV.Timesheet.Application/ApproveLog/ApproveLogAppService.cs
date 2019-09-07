using System.Collections.Generic;
using System.Linq;
using ZNV.Timesheet;
using ZNV.Timesheet.Smtp;
using ZNV.Timesheet.Employee;
using ZNV.Timesheet.Team;
using ZNV.Timesheet.UserSetting;

namespace ZNV.Timesheet.ApproveLog
{
    public class ApproveLogAppService : TimesheetAppServiceBase, IApproveLogAppService
    {
        private readonly IApproveLogRepository _alRepository;
        private readonly IHREmployeeRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserSettingRepository _settingRepository;

        public ApproveLogAppService(IApproveLogRepository alRepository, 
            IHREmployeeRepository userRepository, 
            ITeamRepository teamRepository,
            IUserSettingRepository settingRepository)
        {
            _alRepository = alRepository;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _settingRepository = settingRepository;
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
                if (al.OperateType == "提交" || al.OperateType == "审批通过" || al.OperateType == "驳回" || al.OperateType == "转办")
                {
                    var submitter = _userRepository.GetAll().Where(u => u.EmployeeCode == al.Creator).FirstOrDefault();
                    var setting = _settingRepository.GetAll().Where(s => s.UserId == submitter.EmployeeCode).FirstOrDefault();
                    var team = _teamRepository.Get(setting.TeamId);
                    HREmployee approver = null;

                    if (al.OperateType == "提交")
                    {
                        approver = _userRepository.GetAll().Where(u => u.EmployeeCode == al.NextOperator).FirstOrDefault();
                        EmailSender.SendEmailForSubmitToApprover(team.TeamName, submitter.EmployeeName, approver.EmployeeName, approver.Email, al.Comment, al.OperateTime);
                    }
                    else if (al.OperateType == "审批通过")
                    {
                        approver = _userRepository.GetAll().Where(u => u.EmployeeCode == al.CurrentOperator).FirstOrDefault();
                        EmailSender.SendEmailForApproverCompleteApprove(team.TeamName, submitter.EmployeeName, approver.EmployeeName, approver.Email, al.Comment, al.OperateTime);
                    }
                    else if (al.OperateType == "驳回")
                    {
                        approver = _userRepository.GetAll().Where(u => u.EmployeeCode == al.CurrentOperator).FirstOrDefault();
                        EmailSender.SendEmailForRollbackToSubmitter(team.TeamName, submitter.EmployeeName, approver.EmployeeName, approver.Email, al.Comment, al.OperateTime);
                    }
                    else if (al.OperateType == "转办")
                    {
                        approver = _userRepository.GetAll().Where(u => u.EmployeeCode == al.CurrentOperator).FirstOrDefault();
                        EmailSender.SendEmailForApproverTransferToOther(team.TeamName, submitter.EmployeeName, approver.EmployeeName, approver.Email, al.Comment, al.OperateTime);
                    }
                }
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
