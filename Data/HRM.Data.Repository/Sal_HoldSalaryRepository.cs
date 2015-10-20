using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Sal_HoldSalaryRepository : CustomBaseRepository<Sal_HoldSalary>
    {
        public Sal_HoldSalaryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
