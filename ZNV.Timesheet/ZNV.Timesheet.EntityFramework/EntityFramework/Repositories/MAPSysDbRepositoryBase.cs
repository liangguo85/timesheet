using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace ZNV.Timesheet.EntityFramework.Repositories
{
    public abstract class MAPSysDbRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<MAPSysDbContext, TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>
    {
        protected MAPSysDbRepositoryBase(IDbContextProvider<MAPSysDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
