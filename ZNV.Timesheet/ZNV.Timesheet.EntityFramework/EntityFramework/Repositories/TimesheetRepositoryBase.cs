using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public abstract class TimesheetRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<TimesheetDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected TimesheetRepositoryBase(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class TimesheetRepositoryBase<TEntity> : TimesheetRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected TimesheetRepositoryBase(IDbContextProvider<TimesheetDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
