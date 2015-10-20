using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_WorkPlaceRepository : CustomBaseRepository<Cat_WorkPlace>
    {
        public Cat_WorkPlaceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
