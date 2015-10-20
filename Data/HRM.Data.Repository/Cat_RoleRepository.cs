using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_RoleRepository : CustomBaseRepository<Cat_Role>
    {
        public Cat_RoleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
