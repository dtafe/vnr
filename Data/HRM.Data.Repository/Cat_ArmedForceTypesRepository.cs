using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_ArmedForceTypesRepository : CustomBaseRepository<Cat_ArmedForceTypes>
    {
        public Cat_ArmedForceTypesRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
