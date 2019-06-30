﻿using System;
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

        public List<HRDepartment> GetDepartmentList()
        {
            return _departmentRepository.GetAll().Where(x => x.IsActiveDept == "Y").ToList();
        }
    }
}
