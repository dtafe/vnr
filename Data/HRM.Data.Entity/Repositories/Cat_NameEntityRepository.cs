using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_NameEntityRepository : CustomBaseRepository<Cat_NameEntity>
    {
        public Cat_NameEntityRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
