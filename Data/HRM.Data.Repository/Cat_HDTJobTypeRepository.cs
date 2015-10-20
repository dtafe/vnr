using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_HDTJobTypeRepository : CustomBaseRepository<Cat_HDTJobType>
    {
        public Cat_HDTJobTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
