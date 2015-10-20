using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_ShopRepository : CustomBaseRepository<Cat_Shop>
    {
        public Cat_ShopRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
