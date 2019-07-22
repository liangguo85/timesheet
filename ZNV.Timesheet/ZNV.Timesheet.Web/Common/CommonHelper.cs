using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZNV.Timesheet.Web.Common
{
    public class CommonHelper
    {
        public static string GetProjectNameByProjectID(List<Project.Project> projects, int projectID)
        {
            if (projects != null && projects.Count > 0)
            {
                var w = projects.Where(p => p.Id == projectID).ToList();
                if (w.Count > 0)
                {
                    return w[0].ProjectName;
                }
            }
            return "";
        }
    }
}