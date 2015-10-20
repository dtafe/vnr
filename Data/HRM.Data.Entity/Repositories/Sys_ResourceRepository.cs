using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Sys_ResourceRepository : CustomBaseRepository<Sys_Resource>
    {
        public Sys_ResourceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        //public IQueryable<Sys_Resource> Get()
        //{
        //    IQueryable<Sys_Resource> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}
    }
}
