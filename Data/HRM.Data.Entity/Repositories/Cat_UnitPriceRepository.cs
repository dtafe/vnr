using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_UnitPriceRepository : CustomBaseRepository<Cat_UnitPrice>
    {
        public Cat_UnitPriceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
