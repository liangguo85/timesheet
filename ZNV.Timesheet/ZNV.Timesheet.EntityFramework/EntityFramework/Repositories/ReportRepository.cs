using Abp.EntityFramework;
using ZNV.Timesheet.Report;
using System.Data;
using System.Data.SqlClient;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public class ReportRepository : TimesheetRepositoryBase<ZNV.Timesheet.Holiday.Holiday, int>, IReportRepository
    {
        public ReportRepository(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public DataTable GetDepartmentReport(DepartmentReportSearch search)
        {
            var deptIDs = "";
            if(search.departmentIds != null)
            {
                search.departmentIds.ForEach(item=> {
                    deptIDs += "," + item;
                });
                deptIDs = deptIDs.TrimStart(',');
            }

            DataTable dt = new DataTable();
            EnsureConnectionOpen();

            using (var command = CreateCommand("Proc_DepartmentReport", CommandType.StoredProcedure, 
                new SqlParameter("startDate", search.startDate), new SqlParameter("endDate", search.endDate), 
                new SqlParameter("departmentIDs", deptIDs), new SqlParameter("currentUserID", search.currentUserID)))
            {
                using (var da = new SqlDataAdapter(command)) {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public DataTable GetProjectReport(ProjectReportSearch search)
        {
            var projectIDs = "";
            if (search.projectIds != null)
            {
                search.projectIds.ForEach(item => {
                    projectIDs += "," + item;
                });
                projectIDs = projectIDs.TrimStart(',');
            }

            DataTable dt = new DataTable();
            EnsureConnectionOpen();

            using (var command = CreateCommand("Proc_ProjectReport", CommandType.StoredProcedure,
                new SqlParameter("startDate", search.startDate), new SqlParameter("endDate", search.endDate),
                new SqlParameter("projectIDs", projectIDs), new SqlParameter("currentUserID", search.currentUserID)))
            {
                using (var da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public DataTable GetProjectManpowerReport(ProjectReportSearch search)
        {
            var projectIDs = "";
            if (search.projectIds != null)
            {
                search.projectIds.ForEach(item => {
                    projectIDs += "," + item;
                });
                projectIDs = projectIDs.TrimStart(',');
            }

            DataTable dt = new DataTable();
            EnsureConnectionOpen();

            using (var command = CreateCommand("Proc_ProjectManpowerReport", CommandType.StoredProcedure,
                new SqlParameter("startDate", search.startDate), new SqlParameter("endDate", search.endDate),
                new SqlParameter("projectIDs", projectIDs), new SqlParameter("currentUserID", search.currentUserID)))
            {
                using (var da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public DataTable GetProductionLineReport(ProductionLineReportSearch search)
        {
            var productionLineList = "";
            if (search.productionLineList != null)
            {
                search.productionLineList.ForEach(item => {
                    productionLineList += "," + item;
                });
                productionLineList = productionLineList.TrimStart(',');
            }

            DataTable dt = new DataTable();
            EnsureConnectionOpen();

            using (var command = CreateCommand("Proc_ProductionLineReport", CommandType.StoredProcedure,
                new SqlParameter("startDate", search.startDate), new SqlParameter("endDate", search.endDate),
                new SqlParameter("productionLineList", productionLineList), new SqlParameter("currentUserID", search.currentUserID)))
            {
                using (var da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public DataTable GetTimesheetReport(TimesheetReportSearch search, out int totalCount)
        {
            DataTable dt = new DataTable();
            EnsureConnectionOpen();
            var outputTotalSqlParameter = new SqlParameter("@totalRecords", SqlDbType.Int);
            outputTotalSqlParameter.Direction = ParameterDirection.Output;

            var isPageSqlParameter = new SqlParameter("@isPage", SqlDbType.Bit);
            isPageSqlParameter.Value = search.isPage ? 1 : 0;

            using (var command = CreateCommand("Proc_TimesheetReport", CommandType.StoredProcedure,
                new SqlParameter("startDate", search.startDate), new SqlParameter("endDate", search.endDate),
                new SqlParameter("departmentIDs", search.departmentIds ?? ""), new SqlParameter("productionLineList", search.productionLineList ?? ""),
                new SqlParameter("projectIds", search.projectIds ?? ""), new SqlParameter("userIds", search.userIds ?? ""), new SqlParameter("currentUserID", search.currentUserID),
                isPageSqlParameter, new SqlParameter("pageSize", search.pageSize), new SqlParameter("pageStart", search.pageStart), outputTotalSqlParameter
                ))
            {
                using (var da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }

            totalCount = int.Parse(outputTotalSqlParameter.Value.ToString());

            return dt;
        }

        private SqlCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = Context.Database.Connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return (SqlCommand)command;
        }

        private void EnsureConnectionOpen()
        {
            var connection = Context.Database.Connection;

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// 通过日期列表获取这些日期里面存着未填写周报的员工（返回的表包含EmployeeCode，EmployeeName，[Email]这3个字段）
        /// </summary>
        /// <param name="dateList">日期字符串，多个以英文逗号隔开</param>
        /// <returns>返回的表包含EmployeeCode，EmployeeName，[Email]这3个字段</returns>
        public DataTable GetNotSubmitTimesheetUserList(string dateList)
        {
            DataTable dt = new DataTable();
            EnsureConnectionOpen();
            using (var command = CreateCommand("Proc_GetNotSubmitTimesheetUserList", CommandType.StoredProcedure,
                new SqlParameter("dateList", dateList)))
            {
                using (var da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取所有在职的部门manager
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartmentManagerList()
        {
            DataTable dt = new DataTable();
            EnsureConnectionOpen();
            using (var command = CreateCommand("select * from HRActiveDeptManagerV a where a.FullDeptCode like '10000.11000%' and a.IsActiveDept = 'Y'", CommandType.Text))
            {
                using (var da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
