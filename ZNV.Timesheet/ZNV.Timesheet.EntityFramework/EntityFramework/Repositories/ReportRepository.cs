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
            DataTable dt = new DataTable();
            EnsureConnectionOpen();

            using (var command = CreateCommand("Proc_DepartmentReport", CommandType.StoredProcedure, new SqlParameter("startDate", search.startDate), new SqlParameter("endDate", search.endDate)))
            {
                using (var da = new SqlDataAdapter(command)) {
                    da.Fill(dt);
                }
            }

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
    }
}
