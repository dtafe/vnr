using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_CommunistPartyPositionRepository : CustomBaseRepository<Cat_CommunistPartyPosition>
    {
        public Cat_CommunistPartyPositionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
