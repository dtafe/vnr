using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_SelfDefenceMilitiaPositionRepository : CustomBaseRepository<Cat_SelfDefenceMilitiaPosition>
    {
        public Cat_SelfDefenceMilitiaPositionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
