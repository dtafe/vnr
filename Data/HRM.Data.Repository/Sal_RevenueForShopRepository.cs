using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Sal_RevenueForShopRepository : CustomBaseRepository<Sal_RevenueForShop>
    {
        public Sal_RevenueForShopRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
