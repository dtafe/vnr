using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_YouthUnionPositionRepository : CustomBaseRepository<Cat_YouthUnionPosition>
    {
        public Cat_YouthUnionPositionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
