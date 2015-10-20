using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Sys_LockObjectRepository : CustomBaseRepository<Sys_LockObject>
    {
        public Sys_LockObjectRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
