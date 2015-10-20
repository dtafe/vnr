using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_WoundedSoldierTypesRepository : CustomBaseRepository<Cat_WoundedSoldierTypes>
    {
        public Cat_WoundedSoldierTypesRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
