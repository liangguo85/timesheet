using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNV.Timesheet.Employee
{
    public class EmployeeAppService : TimesheetAppServiceBase, IEmployeeAppService
    {
        private readonly IHREmployeeRepository _employeeRepository;
        private readonly IHRDepartmentRepository _departmentRepository;

        public EmployeeAppService(IHREmployeeRepository employeeRepository, IHRDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public List<HREmployee> GetEmployeeList()
        {
            return _employeeRepository.GetAllList();
        }

        public List<HREmployee> GetEmployeeList(string searchTerm, int pageSize, int pageNum, out int totalCount)
        {
            var current_date = DateTime.Now;
            var tomorrow_date = current_date.AddDays(1);

            totalCount = _employeeRepository.GetAll()
                .Where(
                    x => (string.IsNullOrEmpty(searchTerm) || x.EmployeeName.Contains(searchTerm) || x.EmployeeCode.Contains(searchTerm))
                        && current_date > x.EntryDate && current_date < (x.ExitDate ?? tomorrow_date)
                ).Count();

            var employeeList = (from user in _employeeRepository.GetAll()
                         join department in _departmentRepository.GetAll() on user.DeptCode equals department.DeptCode1
                         where (string.IsNullOrEmpty(searchTerm) || user.EmployeeName.Contains(searchTerm) || user.EmployeeCode.Contains(searchTerm))
                            && current_date > user.EntryDate && current_date < (user.ExitDate ?? tomorrow_date)
                         orderby user.EmployeeName
                         select new {
                             EmployeeCode = user.EmployeeCode,
                             EmployeeName = user.EmployeeName,
                             DeptCode = user.DeptCode,
                             DeptName = department.DeptName1,
                             EntryDate = user.EntryDate,
                             ExitDate = user.ExitDate
                         }).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            var list = new List<HREmployee>();
            employeeList.ForEach(x =>
            {
                list.Add(new HREmployee
                {
                    EmployeeCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,
                    DeptCode = x.DeptCode,
                    DeptName = x.DeptName,
                    EntryDate = x.EntryDate,
                    ExitDate = x.ExitDate
                });
            });

            return list;
        }

        public HREmployee GetEmployeeByCode(string employeeCode)
        {
            return _employeeRepository.GetAll().Where(x => x.EmployeeCode == employeeCode).FirstOrDefault();
        }

        public List<HRDepartment> GetDepartmentList()
        {
            return _departmentRepository.GetAll().Where(x => x.IsActiveDept == "Y").ToList();
        }

        public List<HRDepartment> GetDepartmentList(string searchTerm, int pageSize, int pageNum, out int totalCount)
        {
            var queryable = _departmentRepository.GetAll()
                .Where(x => (string.IsNullOrEmpty(searchTerm) || x.DeptName1.Contains(searchTerm) || x.DeptCode1.Contains(searchTerm)) && x.IsActiveDept == "Y");
            totalCount = queryable.Count();
            return queryable.OrderBy(x=>x.FullDeptName).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
        }

        public HRDepartment GetDepartmentByCode(string departmentCode)
        {
            return _departmentRepository.GetAll().Where(x => x.DeptCode1 == departmentCode).FirstOrDefault();
        }
    }
}
