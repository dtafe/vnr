using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_RateInsuranceRepository : CustomBaseRepository<Cat_RateInsurance>
    {
        public Cat_RateInsuranceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
