using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Sal_LineItemForShopRepository : CustomBaseRepository<Sal_LineItemForShop>
    {
        public Sal_LineItemForShopRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
