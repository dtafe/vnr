using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_VeteranUnionPositionRepository : CustomBaseRepository<Cat_VeteranUnionPosition>
    {
        public Cat_VeteranUnionPositionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
