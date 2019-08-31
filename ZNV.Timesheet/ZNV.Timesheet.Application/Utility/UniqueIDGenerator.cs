using System.Threading;
using Abp.Dependency;
using ZNV.Timesheet.Project;

namespace ZNV.Timesheet.Utility
{
    public static class UniqueIDGenerator
    {
        private static int NextID = IocManager.Instance.Resolve<IProjectAppService>().GetProjectCount();

        public static int GetNextID()
        {
            return Interlocked.Increment(ref NextID);
        }
    }
}
