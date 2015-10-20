using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Sal_ItemForShopRepository : CustomBaseRepository<Sal_ItemForShop>
    {
        public Sal_ItemForShopRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
