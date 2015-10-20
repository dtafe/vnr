using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Sys_AsynTaskRepository : CustomBaseRepository<Sys_AsynTask>
    {
        public Sys_AsynTaskRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        //public IQueryable<Sys_AsynTask> Get()
        //{
        //    IQueryable<Sys_AsynTask> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

    }
}
