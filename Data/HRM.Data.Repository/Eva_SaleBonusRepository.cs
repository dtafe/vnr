using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_SaleBonusRepository : CustomBaseRepository<Eva_SaleBonus>
    {
        public Eva_SaleBonusRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
