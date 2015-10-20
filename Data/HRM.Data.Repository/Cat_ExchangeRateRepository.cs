using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_ExchangeRateRepository : CustomBaseRepository<Cat_ExchangeRate>
    {
        public Cat_ExchangeRateRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        
        
    }
}
