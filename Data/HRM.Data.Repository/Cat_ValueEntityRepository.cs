
using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_ValueEntityRepository : CustomBaseRepository<Cat_ValueEntity>
    {
        public Cat_ValueEntityRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
        

    }
}
