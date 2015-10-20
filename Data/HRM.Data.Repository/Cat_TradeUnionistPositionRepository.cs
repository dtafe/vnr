using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_TradeUnionistPositionRepository : CustomBaseRepository<Cat_TradeUnionistPosition>
    {
        public Cat_TradeUnionistPositionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
